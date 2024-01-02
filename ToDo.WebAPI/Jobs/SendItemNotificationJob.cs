using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.WebAPI.HubClients;

namespace ToDo.Services.Jobs
{
    /// <summary>
    /// Job for sending notification with todo item reminder.
    /// </summary>
    public class SendItemNotificationJob : IJob
    {
        /// <summary>
        /// Key for identifying job.
        /// </summary>
        public static readonly JobKey Key = new JobKey("send-item-notification", "send-notification");

        private readonly ILogger logger;
        private readonly IHubContext<HubClient> hubContext;
        private readonly IRepository<Notification> notificationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendItemNotificationJob"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="hubContext">Hub context for sending notifications using SignalR.</param>
        /// <param name="serviceProvider">Service provider for DI.</param>
        public SendItemNotificationJob(
            ILogger<SendItemNotificationJob> logger,
            IHubContext<HubClient> hubContext,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.hubContext = hubContext;
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
                this.logger.LogInformation($"Job {Key.Name} not succeeded");
                return;
            }

            try
            {
                var notification = JsonConvert.DeserializeObject<Notification>(context.MergedJobDataMap.GetString("notification"));
                notification.SentAt = DateTime.Now;
                notification.NotificationState = NotificationState.Sent;
                this.notificationRepository.Update(notification);
                await this.hubContext.Clients.User(notification.RecipientId.ToString()).SendAsync("item-notification", notification);
                await Task.Delay(100);
                this.logger.LogInformation($"Sending notification {notification.Id} with content {notification} to user {notification.RecipientId}");
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(msg: $"Job {Key.Name} failed", refireImmediately: true, cause: ex);
            }
        }
    }
}
