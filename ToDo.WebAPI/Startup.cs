using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ToDo.DomainModel.Classes;
using ToDo.DomainModel.Context;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Repositories;
using ToDo.Services.Interfaces;
using ToDo.Services.Services;
using ToDo.WebAPI.Context;

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

            var identityConnectionString = this.Configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityConnectionString));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
               x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Configuration.GetSection("JWT").Value)),
                       ValidateAudience = false,
                       ValidateIssuer = false,
                   };
               });

            services.AddControllers();
            services.AddScoped<IRepository<ToDoList>, ToDoListRepository>();
            services.AddScoped<IRepository<ToDoItem>, ToDoItemRepository>();
            services.AddScoped<IToDoItemService, ToDoItemService>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddCors();
        }

        /// <summary>
        /// Main application configuration.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Application environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins(this.Configuration.GetSection("Frontend").Value));

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
