using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ToDo.DomainModel.Classes
{
    /// <summary>
    /// Class representing ToDoList item in database.
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Gets or sets unique ID of <see cref="ToDoList"/> item.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of <see cref="ToDoList"/> item.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets descirption of <see cref="ToDoList"/> item.
        /// </summary>
        [MaxLength(1000)]
        required public string Description { get; set; }

        /// <summary>
        /// Gets or sets date of creation of the <see cref="ToDoList"/>.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets deadline of <see cref="ToDoList"/> item.
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets priority of <see cref="ToDoList"/>.
        /// </summary>
        [Required]
        public Priority Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remind that there is one hour till <see cref="ToDoItem.Deadline"/> left.
        /// </summary>
        public bool Remind { get; set; }

        /// <summary>
        /// Gets or sets Guid of ToDoList this item belongs to.
        /// </summary>
        [Required]
        public int ToDoListID { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ToDoList"/> of ToDoList item it belongs to.
        /// </summary>
        [JsonIgnore]
        public virtual ToDoList? TodoList { get; set; }

        /// <summary>
        /// Gets or sets ItemStatus of <see cref="ToDoList"/> completeness.
        /// </summary>
        [Required]
        public ItemStatus Status { get; set; }
    }
}
