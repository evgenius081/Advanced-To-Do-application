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
        required public string Login { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        required public string Password { get; set; }
    }
}
