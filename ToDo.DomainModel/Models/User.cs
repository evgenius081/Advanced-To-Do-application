using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Models
{
    /// <summary>
    /// Class representing user in database.
    /// </summary>
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        /// <summary>
        /// Gets or sets unique ID of user.
        /// </summary>
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets unique username.
        /// </summary>
        [StringLength(20, MinimumLength =5)]
        [Required]
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets user's hased password.
        /// </summary>
        [Required]
        required public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets list of lists belonging to this user.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<ToDoList> Lists { get; set; } = new List<ToDoList>();
    }
}
