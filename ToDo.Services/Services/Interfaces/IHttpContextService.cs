using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services.Services.Interfaces
{
    /// <summary>
    /// Interface for http context helper.
    /// </summary>
    public interface IHttpContextService
    {
        /// <summary>
        /// Gets logged in user's id.
        /// </summary>
        /// <param name="userContext">Context where to take logged in user's data.</param>
        /// <returns>Logged in user's id.</returns>
        public int GetIdByContextUser(ClaimsPrincipal userContext);
    }
}
