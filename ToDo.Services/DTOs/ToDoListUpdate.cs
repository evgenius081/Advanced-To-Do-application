using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO for <see cref="ToDoList"/> in the moment of updating.
    /// </summary>
    public class ToDoListUpdate
    {
        /// <summary>
        /// Gets or sets unique ID of ToDo list.
        /// </summary>
        required public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of ToDo list. Is not nullable.
        /// </summary>
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
        required public int UserID { get; set; }
    }
}
