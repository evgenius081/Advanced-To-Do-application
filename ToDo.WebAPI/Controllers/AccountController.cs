using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests for account.
    /// </summary>
    [Authorize]
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">Service for signing in/out user.</param>
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Handles request for signing user in.
        /// </summary>
        /// <param name="bodyUser">User login data.</param>
        /// <returns>Ok response if login and password are valid, BadRequest if login, password or model in total is wrong.</returns>
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin bodyUser)
        {
            var tokenString = await this.accountService.Login(bodyUser);
            if (!string.IsNullOrEmpty(tokenString))
            {
                return this.Ok(tokenString);
            }

            return this.BadRequest("Wrong username or password");
        }

        /// <summary>
        /// Handles request for signing user out.
        /// </summary>
        /// <param name="username">Login of person to be logged out.</param>
        /// <returns>Ok response.</returns>
        [Route("Logout/{username}")]
        public async Task<ActionResult> Logout(string username)
        {
            await this.accountService.Logout(username);
            return this.Ok();
        }
    }
}
