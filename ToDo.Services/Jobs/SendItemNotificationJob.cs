using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.DomainModel.Models.NotificationData;
using ToDo.Services.DTOs;
using ToDo.Services.HubClients;

namespace ToDo.Services.Jobs
{
    /// <summary>
    /// Job for sending notification with todo item reminder.
    /// </summary>
    public class SendItemNotificationJob : IJob
    {
        private readonly ILogger logger;
        private readonly IHubContext<HubClient> hubContext;
        private readonly IRepository<Notification> notificationRepository;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendItemNotificationJob"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="hubContext">Hub context for sending notifications using SignalR.</param>
        /// <param name="serviceProvider">Service provider for DI.</param>
        /// <param name="configuration">Configuration.</param>
        public SendItemNotificationJob(
            ILogger<SendItemNotificationJob> logger,
            IHubContext<HubClient> hubContext,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.hubContext = hubContext;
            this.configuration = configuration;
            using (var scope = serviceProvider.CreateScope())
            {
                this.notificationRepository = scope.ServiceProvider.GetRequiredService<IRepository<Notification>>();
            }
        }

        /// <summary>
        /// Updates and sends notification to user using SignalR.
        /// </summary>
        /// <param name="context">Context with job parameters.</param>
        /// <returns>Async void.</returns>
        /// <exception cref="JobExecutionException">Thrown if execute fails to refire job.</exception>
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.RefireCount > 10)
            {
                this.logger.LogInformation($"Job {nameof(SendItemNotificationJob)} not succeeded");
                return;
            }

            try
            {
                this.logger.LogInformation(context.MergedJobDataMap.GetString("notification") !);
                var partialNotification = JsonConvert.DeserializeObject<Notification>(context.MergedJobDataMap.GetString("notification") !, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                //var notificationData = JsonConvert.DeserializeObject<NotificationData>(context.MergedJobDataMap.GetString("notification-data") !);
                //var notification = new Notification
                //{
                //    Id = partialNotification.Id,
                //    NotificationState = NotificationState.Sent,
                //    NotificationType = partialNotification.NotificationType,
                //    RecipientId = partialNotification.RecipientId,
                //    SentAt = DateTime.UtcNow,
                //    NotificationData = notificationData,
                //};
                await this.notificationRepository.InsertAsync(partialNotification);
                await this.hubContext.Clients.User(partialNotification.RecipientId.ToString())
                    .SendAsync(this.configuration.GetSection("Hub:Topic").Value, partialNotification);
                await Task.Delay(100);
                this.logger.LogInformation($"Sending notification {partialNotification.Id} with content {partialNotification} to user {partialNotification.RecipientId}");
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(msg: $"Job {nameof(SendItemNotificationJob)} failed", refireImmediately: true, cause: ex);
            }
        }
    }
}
