using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Interfaces;
using ToDo.Infrastructure.Repositories;
using ToDo.Services.HubClients;
using ToDo.Services.Jobs;
using ToDo.Services.Services;
using ToDo.Services.Services.Interfaces;

namespace ToDo.WebAPI
{
    /// <summary>
    /// Start up class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration object.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration for solution.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures services for entire application.
        /// </summary>
        /// <param name="services">Collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var todoConnectionString = this.Configuration.GetConnectionString("ToDoConnection");
            services.AddDbContext<IApplicationContext, ApplicationContext>(options =>
                options.UseSqlServer(todoConnectionString));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
               x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Configuration.GetSection("JWT:Secret").Value)),
                       ValidateAudience = true,
                       ValidAudience = this.Configuration.GetSection("JWT:ValidAudience").Value!,
                       ValidateIssuer = true,
                       ValidIssuer = this.Configuration.GetSection("JWT:ValidIssuer").Value!,
                       ClockSkew = TimeSpan.Zero,
                   };
               });

            services.AddControllers();
            services.AddScoped<IRepository<ToDoList>, ToDoListRepository>();
            services.AddScoped<IRepository<ToDoItem>, ToDoItemRepository>();
            services.AddScoped<IRepository<Notification>, NotificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IToDoItemService, ToDoItemService>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IHttpContextService, HttpContextService>();

            services.AddHttpContextAccessor();

            services.AddCors();

            services.AddLogging();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
                hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(5);
            });

            services.AddQuartz(q =>
            {
                q.SchedulerName = this.Configuration.GetSection("Quartz:SchedulerId").Value;

                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 20;
                });

                var checkNewItemsJobKey = new JobKey(nameof(CheckForNewNotificationsJob));

                q.AddJob<CheckForNewNotificationsJob>(checkNewItemsJobKey);

                q.AddTrigger(t => t
                    .WithIdentity(this.Configuration.GetSection("Quartz:Triggers:CheckNotifications").Value)
                    .ForJob(checkNewItemsJobKey)
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(30)).RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        /// <summary>
        /// Main application configuration.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Application environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var applicationContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                applicationContext.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedData.EnsurePopulated(
                    app.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<ApplicationContext>());
            }

            app.UseHttpsRedirection();
            app.UseCors(x =>
            {
                x.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((x) => true)
                .WithOrigins(this.Configuration.GetSection("Frontend:Endpoint").Value);
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<HubClient>(this.Configuration.GetSection("Hub:Endpoint").Get<string>(), options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
