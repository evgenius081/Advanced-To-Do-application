using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Identity.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IAccountService"/> interface.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="userManager">Manager for user in persistance store.</param>
        /// <param name="signInManager">Manager for signing in/out user.</param>
        /// <param name="configuration">Configuration.</param>
        public AccountService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<string?> Login(UserLogin userLogin)
        {
            IdentityUser? user = await userManager.FindByNameAsync(userLogin.Login);

            if (user != null)
            {
                await signInManager.SignOutAsync();

                if ((await signInManager.PasswordSignInAsync(user, userLogin.Password, false, false)).Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JWT").Value!));
                    var signingCreds = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
                    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                        SigningCredentials = signingCreds,
                        Expires = DateTime.UtcNow.AddDays(7),
                    });

                    var tokenString = tokenHandler.WriteToken(token);
                    await userManager.SetAuthenticationTokenAsync(user, "JWT", "JWT Token", tokenString);

                    return tokenString;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task Logout(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user is null)
            {
                return;
            }
            await userManager.UpdateSecurityStampAsync(user);
            await userManager.RemoveAuthenticationTokenAsync(user, "JWT", "JWT Token");
        }
    }
}
