using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Enums
{
    /// <summary>
    /// Represents possible states of <see cref="Notification"/>.
    /// </summary>
    public enum NotificationState
    {
        /// <summary>
        /// Notification is created.
        /// </summary>
        Created,

        /// <summary>
        /// Notification is sent to the user.
        /// </summary>
        Sent,

        /// <summary>
        /// Notification is read by the user.
        /// </summary>
        Read,
    }
}
