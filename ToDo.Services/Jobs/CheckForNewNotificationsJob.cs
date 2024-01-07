using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Models;
using ToDo.DomainModel.Models.NotificationData;
using ToDo.Services.DTOs;
using ToDo.Services.Services.Interfaces;

namespace ToDo.Services.Jobs
{
    /// <summary>
    /// Periodic job for checking if some notifications should be sent now.
    /// </summary>
    public class CheckForNewNotificationsJob : IJob
    {
        private readonly INotificationService notificationService;
        private readonly IToDoItemService todoItemService;
        private readonly IToDoListService toDoListService;
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
            IConfiguration configuration,
            INotificationService notificationService,
            IToDoItemService itemService,
            IToDoListService listService)
        {

            this.logger = logger;
            this.notificationService = notificationService;
            todoItemService = itemService;
            toDoListService = listService;
            this.configuration = configuration;
            this.logger.LogInformation($"Job {nameof(CheckForNewNotificationsJob)} started");
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
                logger.LogInformation($"Job {nameof(CheckForNewNotificationsJob)} not succeeded");
                return;
            }

            try
            {
                var schedulerFactory = new StdSchedulerFactory();
                schedulerFactory.Initialize();
                var scheduler = await schedulerFactory.GetScheduler(configuration.GetSection("Quartz:SchedulerId").Value);

                var lists = toDoListService.GetAllLists();
                var listsIdsToNotify = lists
                    .Select(l => l.Id);
                var itemsIdsWithNotifications = notificationService.GetAll()
                    .Where(n => n.NotificationType == NotificationType.ReminderNotificationType)
                    .Select(n => ((ReminderNotificationData)n.NotificationData).ToDoItemId);
                var itemsToNotify = todoItemService.GetItemsForReminder()
                    .Where(i => listsIdsToNotify.Contains(i.ToDoListID)
                    && !itemsIdsWithNotifications.Contains(i.Id));

                foreach (var item in itemsToNotify)
                {
                    var list = lists.FirstOrDefault(l => l.Id == item.ToDoListID);
                    await CreateAndSendNotification(item, scheduler, list);
                }
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(msg: $"Job {nameof(CheckForNewNotificationsJob)} failed", refireImmediately: true, cause: ex);
            }
        }

        private async Task CreateAndSendNotification(ToDoItem item, IScheduler scheduler, ToDoList list)
        {
            var notificationData = new ReminderNotificationData
            {
                ToDoItemId = item.Id,
                Deadline = item.Deadline,
                ToDoListId = item.ToDoListID,
                ToDoItemName = item.Title,
                ToDoListName = list.Title,
            };
            var notification = new Notification
            {
                NotificationData = notificationData,
                NotificationState = NotificationState.Created,
                NotificationType = NotificationType.ReminderNotificationType,
                RecipientId = list.UserID,
                SentAt = DateTime.UtcNow,
            };
            var partialNotificationJobData = new JobDataMap { { "notification", JsonConvert.SerializeObject(notification, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) } };
            //var notificationDataJobData = new JobDataMap { { "notification-data", JsonConvert.SerializeObject(notificationData) } };
            var sendNotificationJobKey = new JobKey(Guid.NewGuid().ToString());
            var job = JobBuilder.Create<SendItemNotificationJob>()
                .WithIdentity(sendNotificationJobKey)
                .UsingJobData(partialNotificationJobData)
                //.UsingJobData(notificationDataJobData)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString())
                .ForJob(sendNotificationJobKey)
                .StartAt(item.Deadline)
                .Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
