using ToDo.DomainModel.Enums;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO representing change of notification state.
    /// </summary>
    public class NotificationStateUpdate
    {
        /// <summary>
        /// Gets or sets notification id.
        /// </summary>
        required public int Id { get; set; }

        /// <summary>
        /// Gets or sets notification state.
        /// </summary>
        required public NotificationState NotificationState { get; set; }
    }
}
