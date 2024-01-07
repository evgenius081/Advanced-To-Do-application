using ToDo.DomainModel.Attributes;
using ToDo.DomainModel.Models.NotificationData;

namespace ToDo.DomainModel.Enums
{
    /// <summary>
    /// Represents notification type using name of notification data class.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Represents notification that is sent 1 hour before todo item deadline.
        /// </summary>
        [StringValue(nameof(ReminderNotificationData))]
        ReminderNotificationType = 1,
    }
}
