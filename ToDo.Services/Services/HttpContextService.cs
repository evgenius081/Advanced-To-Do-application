using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Class implementing <see cref="IHttpContextService"/> interface.
    /// </summary>
    public class HttpContextService : IHttpContextService
    {
        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown if http context does not have user id.</exception>
        public int GetIdByContextUser(ClaimsPrincipal userContext)
        {
            var nameIdentifierClaim = userContext.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim != null)
            {
                if (int.TryParse(nameIdentifierClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            throw new ArgumentException("No User id in HttpContext User");
        }
    }
}
