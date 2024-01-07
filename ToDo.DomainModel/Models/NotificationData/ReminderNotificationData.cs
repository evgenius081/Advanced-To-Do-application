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
        required public int ToDoItemId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="TodoItem.Name" />.
        /// </summary>
        [Required]
        required public string ToDoItemName { get; set; }

        /// <summary>
        /// Gets or sets id of <see cref="ToDoList"/> item belongs to.
        /// </summary>
        [Required]
        required public int ToDoListId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="TodoList.Name" /> of the list item belongs to.
        /// </summary>
        [Required]
        required public string ToDoListName { get; set; }

        /// <summary>
        /// Gets or sets deadline of <see cref="ToDoItem"/> item.
        /// </summary>
        [Required]
        required public DateTime Deadline { get; set; }
    }
}
