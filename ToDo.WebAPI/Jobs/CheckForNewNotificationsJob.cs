using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.DomainModel.Models.NotificationData;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Jobs
{
    /// <summary>
    /// Periodic job for checking if some notifications should be sent now.
    /// </summary>
    public class CheckForNewNotificationsJob : IJob
    {
        private readonly INotificationService notificationService;
        private readonly IToDoItemService todoItemService;
        private readonly IToDoListService toDoListService;
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHttpContextService httpContextService;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckForNewNotificationsJob"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider for DI.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">Configuration.</param>
        public CheckForNewNotificationsJob(
            IServiceProvider serviceProvider,
            ILogger<CheckForNewNotificationsJob> logger,
            IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                this.notificationService = scope.ServiceProvider.GetService<INotificationService>();
                this.todoItemService = scope.ServiceProvider.GetService<IToDoItemService>();
                this.toDoListService = scope.ServiceProvider.GetService<IToDoListService>();
                this.httpContextService = scope.ServiceProvider.GetService<IHttpContextService>();
                this.userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            }

            this.logger = logger;

            this.configuration = configuration;

            this.Key = new JobKey(
                this.configuration.GetSection("Quartz:Groups:Check:CheckItemNotifications").Value,
                this.configuration.GetSection("Quartz:Groups:Check:Name").Value);
        }

        /// <summary>
        /// Gets key for identifying job.
        /// </summary>
        public JobKey Key { get; }

        /// <summary>
        /// Checks if during next hour todo item reminder notifications should be sent, and if so, sends them.
        /// </summary>
        /// <param name="context">Context with job parameters.</param>
        /// <returns>Async void.</returns>
        /// <exception cref="JobExecutionException">Thrown if execute fails to refire job.</exception>
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.RefireCount > 10)
            {
                this.logger.LogInformation($"Job {this.Key.Name} not succeeded");
                return;
            }

            try
            {
                var schedulerFactory = new StdSchedulerFactory();
                schedulerFactory.Initialize();
                var scheduler = await schedulerFactory.GetScheduler(this.configuration.GetSection("Quartz:SchedulerId").Value);

                var userId = this.httpContextService.GetIdByContextUser(this.httpContextAccessor.HttpContext.User);
                var user = await this.userRepository.GetByID(userId);

                var listsIdsToNotifyForCurrentUser = this.toDoListService.GetAllLists()
                    .Where(l => l.UserID == userId)
                    .Select(l => l.Id);
                var itemsIdsWithNotifications = this.notificationService.GetByUserId(userId)
                    .Where(n => n.NotificationType == NotificationType.ReminderNotificationType)
                    .Select(n => ((ReminderNotificationData)n.NotificationData).ToDoItemID);
                var itemsToNotifyForCurrentUser = this.todoItemService.GetItemsForReminder()
                    .Where(i => listsIdsToNotifyForCurrentUser.Contains(i.ToDoListID)
                    && !itemsIdsWithNotifications.Contains(i.Id));

                foreach (var item in itemsToNotifyForCurrentUser)
                {
                    await this.CreateAndSendNotification(item, scheduler, user);
                }
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(msg: $"Job {this.Key.Name} failed", refireImmediately: true, cause: ex);
            }
        }

        private async Task CreateAndSendNotification(ToDoItem item, IScheduler scheduler, User user)
        {
            var notificationData = new ReminderNotificationData
            {
                TodoItem = item,
                ToDoItemID = item.Id,
                Deadline = item.Deadline,
                TodoList = item.TodoList,
                ToDoListID = item.ToDoListID,
            };
            var notification = new Notification
            {
                NotificationData = notificationData,
                NotificationState = NotificationState.Created,
                NotificationType = NotificationType.ReminderNotificationType,
                RecipientId = user.Id,
                SentAt = DateTime.UtcNow,
                Recipient = user,
            };
            var jobData = new JobDataMap { { "notification", JsonConvert.SerializeObject(notification) } };
            var trigger = TriggerBuilder.Create()
                .WithIdentity(this.configuration.GetSection("Quartz:Triggers:SendNotification").Value)
                .ForJob(new JobKey(
                    this.configuration.GetSection("Quartz:Groups:SendNotification:SendItemNotification").Value,
                    this.configuration.GetSection("Quartz:Groups:SendNotification:Name").Value))
                .StartAt(item.Deadline)
                .UsingJobData(jobData)
                .Build();
            await scheduler.ScheduleJob(trigger);
        }
    }
}
