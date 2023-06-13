using System.ComponentModel.DataAnnotations;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO for IdentityUser.
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Gets or sets user login.
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        public required string Password { get; set; }
    }
}
