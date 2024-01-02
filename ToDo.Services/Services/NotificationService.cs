using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="INotificationService"/> interface.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> notificationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notificationRepository">Notification repository.</param>
        public NotificationService(IRepository<Notification> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        /// <inheritdoc />
        public Task<Notification> CreateNotificationAsync(Notification notification)
        {
            return this.notificationRepository.InsertAsync(notification);
        }

        /// <inheritdoc />
        public void Delete(int notificationId)
        {
            this.notificationRepository.Delete(notificationId);
        }

        /// <inheritdoc />
        public IEnumerable<Notification> GetAll()
        {
            return this.notificationRepository.GetAll();
        }

        /// <inheritdoc />
        public IEnumerable<Notification> GetByUserId(int userId)
        {
            return this.notificationRepository.GetAll().Where(n => n.RecipientId == userId);
        }

        /// <inheritdoc />
        public async Task<Notification?> GetNotificationAsync(int notificationId)
        {
            return await this.notificationRepository.GetByID(notificationId);
        }

        /// <inheritdoc />
        public async Task<Notification> UpdateNotificationStateAsync(NotificationStateUpdate dto)
        {
            var notification = await this.GetNotificationAsync(dto.Id);
            if (notification == null)
            {
                throw new ArgumentException(nameof(dto));
            }

            notification.NotificationState = dto.NotificationState;
            this.notificationRepository.Update(notification);
            return notification;
        }
    }
}
