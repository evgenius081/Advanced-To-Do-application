using System;
using ToDo.DomainModel.Models.NotificationData;

namespace ToDo.DomainModel.Enums
{
    public record NotificationType : Enumeration<NotificationType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationType"/> class.
        /// </summary>
        /// <param name="id">Enumeration item id.</param>
        /// <param name="type">Enumeration item value.</param>
        public NotificationType(int id, Type type)
        : base(id, type)
        {
        }

        /// <summary>
        /// Reminder notification type.
        /// </summary>
        public static NotificationType ReminderNotificationType = new (1, typeof(ReminderNotificationData));
    }
}
