using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ToDoApplication.DTOs;
using ToDoApplication.Services.Interfaces;

namespace ToDoApplication.Services
{
    /// <summary>
    /// Service implementing <see cref="IAccountService"/> interface.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="userManager">Manager for user in persistance store.</param>
        /// <param name="signInManager">Manager for signing in/out user.</param>
        public AccountService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <inheritdoc/>
        public async Task<string> Login(UserLogin userLogin)
        {
            IdentityUser user = await this.userManager.FindByNameAsync(userLogin.Login);

            if (user != null)
            {
                await this.signInManager.SignOutAsync();

                if ((await this.signInManager.PasswordSignInAsync(user, userLogin.Password, false, false)).Succeeded)
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

                    return tokenString;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task Logout(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            await this.userManager.UpdateSecurityStampAsync(user);
            await this.userManager.RemoveAuthenticationTokenAsync(user, "JWT", "JWT Token");
        }
    }
}
