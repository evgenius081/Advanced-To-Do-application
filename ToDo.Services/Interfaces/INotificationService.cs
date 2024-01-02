using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Service for <see cref="Notification"/>.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Returns all notifications from database.
        /// </summary>
        /// <returns>List of all notifications.</returns>
        IEnumerable<Notification> GetAll();

        /// <summary>
        /// Returns all notifications for given user.
        /// </summary>
        /// <param name="userId">Id of a user to serach by.</param>
        /// <returns>List of notifications.</returns>
        IEnumerable<Notification> GetByUserId(int userId);

        /// <summary>
        /// Returns single notification by id.
        /// </summary>
        /// <param name="notificationId">Notification id to search by.</param>
        /// <returns>Notification with given id, null otherwise.</returns>
        Task<Notification?> GetNotificationAsync(int notificationId);

        /// <summary>
        /// Creates notification in database.
        /// </summary>
        /// <param name="notification">Notification to create.</param>
        /// <returns>Created notification.</returns>
        Task<Notification> CreateNotificationAsync(Notification notification);

        /// <summary>
        /// Updates notification state in database.
        /// </summary>
        /// <param name="dto">DTO with changed notification state.</param>
        /// <returns>Updated notification.</returns>
        Task<Notification> UpdateNotificationStateAsync(NotificationStateUpdate dto);

        /// <summary>
        /// Deletes notification from database by id.
        /// </summary>
        /// <param name="notificationId">Notification id to delete.</param>
        void Delete(int notificationId);
    }
}
