using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IUserService"/> interface.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for <see cref="User"/>.</param>
        /// <param name="passwordHasher">Service for hashing and verifing password.</param>
        /// <param name="configuration">Configuration.</param>
        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration,
            IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.passwordHasher = passwordHasher;
        }

        /// <inheritdoc/>
        public async Task<string?> Login(UserLogin userLogin)
        {
            var user = await this.userRepository.GetByUsernameAsync(userLogin.Login);

            if (user == null || !this.passwordHasher.VerifyPassword(user.PasswordHash, userLogin.Password))
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.configuration.GetSection("JWT").Value!));
            var signingCreds = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
                SigningCredentials = signingCreds,
                Expires = DateTime.UtcNow.AddDays(1),
            });

            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        /// <inheritdoc/>
        public Task Logout(string username)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<string?> Register(UserLogin userLogin)
        {
            var user = await this.userRepository.GetByUsernameAsync(userLogin.Login);

            if (user != null || userLogin == null)
            {
                return string.Empty;
            }

            var newUser = new User()
            {
                Username = userLogin.Login,
                PasswordHash = this.passwordHasher.HashPassword(userLogin.Password),
            };

            newUser = await this.userRepository.InsertAsync(newUser);

            var tokenHandler = new JwtSecurityTokenHandler();
            var seckey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.configuration.GetSection("JWT").Value!));
            var signingCreds = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()) }),
                SigningCredentials = signingCreds,
                Expires = DateTime.UtcNow.AddDays(1),
            });

            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
