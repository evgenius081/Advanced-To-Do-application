using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Services.Interfaces;

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
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for <see cref="User"/>.</param>
        /// <param name="passwordHasher">Service for hashing and verifing password.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="tokenService">Service for handling JWT tokens.</param>
        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
        }

        /// <inheritdoc/>
        public async Task<Token?> Login(UserLogin userLogin)
        {
            var user = await this.userRepository.GetByUsernameAsync(userLogin.Login);

            if (user == null || !this.passwordHasher.VerifyPassword(user.PasswordHash, userLogin.Password))
            {
                return null;
            }

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = this.tokenService.CreateToken(authClaims);
            var refreshToken = this.tokenService.GenerateRefreshToken();

            _ = int.TryParse(this.configuration.GetSection("JWT:RefreshTokenValidityInDays").Value!, out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpire = DateTime.Now.AddDays(refreshTokenValidityInDays);

            this.userRepository.Update(user);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new Token() { AccessToken = tokenString, RefreshToken = refreshToken };
        }

        /// <inheritdoc/>
        public Task Logout(string username)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<User?> Register(UserLogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException(nameof(userLogin));
            }

            var user = await this.userRepository.GetByUsernameAsync(userLogin.Login);

            if (user != null)
            {
                return null;
            }

            var newUser = new User()
            {
                Username = userLogin.Login,
                PasswordHash = this.passwordHasher.HashPassword(userLogin.Password),
                RefreshTokenExpire = DateTime.UtcNow.AddDays(1),
            };

            return await this.userRepository.InsertAsync(newUser);
        }
    }
}
