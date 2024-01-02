using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ToDo.DomainModel.Enums;

namespace ToDo.DomainModel.Models
{
    /// <summary>
    /// Class representing notification in database.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets unique notification id.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets date and time of sending <see cref="Notification"/>.
        /// </summary>
        [Required]
        required public DateTime SentAt { get; set; }

        /// <summary>
        /// Gets or sets notification data depending on notification type.
        /// </summary>
        required public NotificationData.NotificationData NotificationData { get; set; }

        /// <summary>
        /// Gets or sets notification type.
        /// </summary>
        required public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets recipient id.
        /// </summary>
        required public int RecipientId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="User"/> who is the recepient of notification.
        /// </summary>
        [Required]
        [JsonIgnore]
        public virtual User? Recipient { get; set; }

        /// <summary>
        /// Gets or sets notification state.
        /// </summary>
        required public NotificationState NotificationState { get; set; }
    }
}
