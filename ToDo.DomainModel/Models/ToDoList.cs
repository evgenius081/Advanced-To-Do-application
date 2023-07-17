using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDo.DomainModel.Classes
{
    /// <summary>
    /// Class representing ToDoList object in database.
    /// </summary>
    public class ToDoList
    {
        /// <summary>
        /// Gets or sets unique ID of ToDo list.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of ToDo list. Is not nullable.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ToDoList"/> is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets list of tasks belonging to this list.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<ToDoItem> Items { get; set; } = new List<ToDoItem>();
    }
}
