using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDo.DomainModel.Interfaces;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Class implementing <see cref="ITokenService"/>.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration class.</param>
        /// <param name="userRepository">Repository for <see cref="User"/> objects.</param>
        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        /// <inheritdoc />
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <inheritdoc />
        /// <exception cref="SecurityTokenException">Thrown if token is not valid.</exception>
        public ClaimsPrincipal? GetPrincipalFromToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("JWT:Secret").Value!)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        /// <inheritdoc />
        public JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("JWT:Secret").Value!));
            _ = int.TryParse(this.configuration.GetSection("JWT:TokenValidityInMinutes").Value!, out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: this.configuration.GetSection("JWT:ValidIssuer").Value!,
                audience: this.configuration.GetSection("JWT:ValidAudience").Value!,
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        /// <inheritdoc />
        public async Task<Token?> RefreshToken(Token token)
        {
            var principal = this.GetPrincipalFromToken(token.AccessToken);

            if (principal == null)
            {
                return null;
            }

            if (principal.Identity == null || principal.Identity.Name == null)
            {
                return null;
            }

            string username = principal.Identity.Name;

            var user = await this.userRepository.GetByUsernameAsync(username);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpire <= DateTime.Now)
            {
                return null;
            }

            var newAccessToken = this.CreateToken(principal.Claims.ToList());
            var newRefreshToken = this.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            this.userRepository.Update(user);

            return new Token { RefreshToken = newRefreshToken, AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken) };
        }
    }
}
