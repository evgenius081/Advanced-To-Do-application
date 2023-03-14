using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoApplication.DTOs;

namespace ToDoApplication.Controllers
{
    /// <summary>
    /// Controller handling requests for account.
    /// </summary>
    [Authorize]
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">Manager for user in persistance store.</param>
        /// <param name="signInManager">Manager for signing in/out user.</param>
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            IdentityUser user = await this.userManager.FindByNameAsync(bodyUser.Login);

            if (user != null)
            {
                await this.signInManager.SignOutAsync();

                if ((await this.signInManager.PasswordSignInAsync(user, bodyUser.Password, false, false)).Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("A_VERY_SECRET_SECURITY_KEY_FOR_JWT_AUTH"));
                    var signingCreds = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
                    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                        SigningCredentials = signingCreds,
                        Expires = DateTime.UtcNow.AddDays(7),
                    });

                    var tokenString = tokenHandler.WriteToken(token);

                    await this.userManager.SetAuthenticationTokenAsync(await this.userManager.FindByNameAsync(user.UserName), "JWT", "JWT Token", tokenString);

                    return this.Ok(tokenString);
                }
            }

            return this.BadRequest("Invalid login or password.");
        }

        /// <summary>
        /// Handles request for signing user out.
        /// </summary>
        /// <param name="username">Login of person to be logged out.</param>
        /// <returns>Ok response.</returns>
        [Route("Logout/{username}")]
        public async Task<ActionResult> Logout(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            await this.userManager.UpdateSecurityStampAsync(user);
            await this.userManager.RemoveAuthenticationTokenAsync(user, "JWT", "JWT Token");
            return this.Ok();
        }
    }
}
