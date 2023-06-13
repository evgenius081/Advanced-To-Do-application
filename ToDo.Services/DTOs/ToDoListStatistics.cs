using System.ComponentModel.DataAnnotations;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO for <see cref="ToDoList"/> when item statistics is needed.
    /// </summary>
    public class ToDoListStatistics
    {
        /// <summary>
        /// Gets or sets unique ID of ToDo list.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of ToDo list. Is not nullable.
        /// </summary>
        [Required]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ToDoList"/> is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.NotStarted"/>.
        /// </summary>
        public int ItemsNotStarted { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.InProcess"/>.
        /// </summary>
        public int ItemsInProcess { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.Completed"/>.
        /// </summary>
        public int ItemsCompleted { get; set; }
    }
}
