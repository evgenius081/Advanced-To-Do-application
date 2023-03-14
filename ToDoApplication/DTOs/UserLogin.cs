using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApplication.DTOs
{
    /// <summary>
    /// DTO for IdentityUser.
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Gets or sets user login.
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
