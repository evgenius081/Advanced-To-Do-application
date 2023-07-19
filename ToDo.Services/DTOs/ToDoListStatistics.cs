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
        required public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of ToDo list. Is not nullable.
        /// </summary>
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ToDoList"/> is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.NotStarted"/>.
        /// </summary>
        required public int ItemsNotStarted { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.InProcess"/>.
        /// </summary>
        required public int ItemsInProcess { get; set; }

        /// <summary>
        /// Gets or sets the number of items that has status <see cref="ItemStatus.Completed"/>.
        /// </summary>
        required public int ItemsCompleted { get; set; }

        /// <summary>
        /// Gets or sets id of user this list belongs to.
        /// </summary>
        required public int UserID { get; set; }
    }
}
