using System.ComponentModel.DataAnnotations;
using ToDo.DomainModel.Models;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO representing <see cref="ToDoItem"/> in the moment of updating.
    /// </summary>
    public class ToDoItemUpdate
    {
        /// <summary>
        /// Gets or sets unique ID of <see cref="ToDoList"/> item.
        /// </summary>
        required public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of <see cref="ToDoList"/> item.
        /// </summary>
        [MinLength(5)]
        [MaxLength(50)]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets descirption of <see cref="ToDoList"/> item.
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets date of creation of the <see cref="ToDoList"/>.
        /// </summary>
        required public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets deadline of <see cref="ToDoList"/> item.
        /// </summary>
        [Required]
        required public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets priority of <see cref="ToDoList"/>.
        /// </summary>
        [Required]
        required public Priority Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remind that there is one hour till <see cref="ToDoItem.Deadline"/> left.
        /// </summary>
        required public bool Remind { get; set; }

        /// <summary>
        /// Gets or sets id of ToDoList this item belongs to.
        /// </summary>
        [Required]
        required public int ToDoListID { get; set; }

        /// <summary>
        /// Gets or sets ItemStatus of <see cref="ToDoList"/> completeness.
        /// </summary>
        [Required]
        required public ItemStatus Status { get; set; }
    }
}
