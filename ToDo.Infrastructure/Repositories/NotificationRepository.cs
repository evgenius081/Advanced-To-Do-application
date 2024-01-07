using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Infrastructure.Interfaces;

namespace ToDo.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for <see cref="Notification"/> object in database.
    /// </summary>
    public class NotificationRepository : IRepository<Notification>
    {
        private readonly IApplicationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        /// <param name="context"><see cref="IApplicationContext"/> object.</param>
        /// <exception cref="ArgumentNullException">Thrown if context is null.</exception>
        public NotificationRepository(IApplicationContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context), "Context must not be null.");
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thown if notification with given id does not exist in dataase.</exception>
        public void Delete(int id)
        {
            var notification = this.context.Notifications.SingleOrDefault(x => x.Id == id) ??
                throw new ArgumentException("There is no such object in database with this id.");

            this.context.Notifications.Remove(notification);
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        public IEnumerable<Notification> GetAll()
        {
            return this.context.Notifications;
        }

       /// <inheritdoc />
        public async Task<Notification?> GetByID(int id)
        {
            return await this.context.Notifications.SingleOrDefaultAsync(i => i.Id == id);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if notification parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown if notification alreay exists in database, or if notification does not have recipient assigned, or if recipient ids do not match.</exception>
        public void Update(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification object must not be null.");
            }

            var entryFound = this.context.Notifications.SingleOrDefault(n => n.Id == notification.Id) ?? throw new ArgumentException("There is no such Notification in database.");
            entryFound.NotificationData = notification.NotificationData;
            entryFound.NotificationType = notification.NotificationType;
            entryFound.RecipientId = notification.RecipientId;
            entryFound.NotificationState = notification.NotificationState;
            entryFound.SentAt = notification.SentAt;
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if notification parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown if notification alreay exists in database, or if notification does not have recipient assigned, or if recipient ids do not match.</exception>
        public Task<Notification> InsertAsync(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification object must not be null.");
            }

            if (this.context.Notifications.Any(e => e.Id == notification.Id))
            {
                throw new ArgumentException("Notification is already in database.");
            }

            return this.InsertAsyncAsync(notification);
        }

        /// <summary>
        /// Method for async InsertAsyncing to database if <see cref="ToDoItemRepository.InsertAsync(ToDoItem)"/> checks passed.
        /// </summary>
        /// <param name="notification"><see cref="Notification"/> to be InsertAsynced.</param>
        /// <returns>Task.</returns>
        private async Task<Notification> InsertAsyncAsync(Notification notification)
        {
            await this.context.Notifications.AddAsync(notification);
            this.context.SaveChanges();
            return notification;
        }
    }
}
