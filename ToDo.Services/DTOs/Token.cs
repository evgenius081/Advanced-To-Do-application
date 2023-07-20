using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services.DTOs
{
    /// <summary>
    /// DTO with generated JWT token and refresh token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets or sets generated token.
        /// </summary>
        required public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets refresh token.
        /// </summary>
        required public string RefreshToken { get; set; }
    }
}
