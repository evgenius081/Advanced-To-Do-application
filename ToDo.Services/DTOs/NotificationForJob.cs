using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.DomainModel.Enums;

namespace ToDo.Services.DTOs
{
    public class NotificationForJob
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
        /// Gets or sets notification type.
        /// </summary>
        required public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets recipient id.
        /// </summary>
        required public int RecipientId { get; set; }

        /// <summary>
        /// Gets or sets notification state.
        /// </summary>
        required public NotificationState NotificationState { get; set; }
    }
}
