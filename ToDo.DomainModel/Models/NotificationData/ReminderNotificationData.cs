using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDo.DomainModel.Models.NotificationData
{
    /// <summary>
    /// Class representing notification that <see cref="ToDoItem.Deadline"/> is in hour.
    /// </summary>
    public class ReminderNotificationData : NotificationData
    {
        /// <summary>
        /// Gets or sets id of <see cref="ToDoItem"/> notification belongs to.
        /// </summary>
        [Required]
        required public int ToDoItemID { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ToDoItem"/> notification belongs to.
        /// </summary>
        [Required]
        [JsonIgnore]
        public virtual ToDoItem? TodoItem { get; set; }

        /// <summary>
        /// Gets or sets id of <see cref="ToDoList"/> item belongs to.
        /// </summary>
        [Required]
        required public int ToDoListID { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ToDoList"/> of ToDoList item it belongs to.
        /// </summary>
        [Required]
        [JsonIgnore]
        public virtual ToDoList? TodoList { get; set; }

        /// <summary>
        /// Gets or sets deadline of <see cref="ToDoItem"/> item.
        /// </summary>
        [Required]
        required public DateTime Deadline { get; set; }
    }
}
