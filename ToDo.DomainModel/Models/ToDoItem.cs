using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ToDo.DomainModel.Enums;

namespace ToDo.DomainModel.Models
{
    /// <summary>
    /// Class representing ToDoList item in database.
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Gets or sets unique ID of <see cref="ToDoItem"/> item.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of <see cref="ToDoItem"/> item.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets descirption of <see cref="ToDoItem"/> item.
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets date of creation of the <see cref="ToDoItem"/>.
        /// </summary>
        [Required]
        required public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets deadline of <see cref="ToDoItem"/> item.
        /// </summary>
        [Required]
        required public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets priority of <see cref="ToDoItem"/>.
        /// </summary>
        [Required]
        required public Priority Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remind that there is one hour till <see cref="ToDoItem.Deadline"/> left.
        /// </summary>
        required public bool Remind { get; set; }

        /// <summary>
        /// Gets or sets id of <see cref="ToDoList"/> this item belongs to.
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
        /// Gets or sets ItemStatus of <see cref="ToDoItem"/> completeness.
        /// </summary>
        [Required]
        required public ItemStatus Status { get; set; }
    }
}
