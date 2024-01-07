using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Models
{
    /// <summary>
    /// Class representing ToDoList object in database.
    /// </summary>
    [Table("lists")]
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
        required public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets id of user this list belongs to.
        /// </summary>
        [Required]
        required public int UserID { get; set; }

        /// <summary>
        /// Gets or sets user this list belongs to.
        /// </summary>
        [Required]
        [JsonIgnore]
        public virtual User? User { get; set; }

        /// <summary>
        /// Gets or sets list of tasks belonging to this list.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<ToDoItem> Items { get; set; } = new List<ToDoItem>();
    }
}
