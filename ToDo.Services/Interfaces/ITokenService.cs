using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo.Services.DTOs;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Service for handling JWT tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates refresh token.
        /// </summary>
        /// <returns>Generated refresh token.</returns>
        public string GenerateRefreshToken();

        /// <summary>
        /// Gets princimapl from token.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <returns>Prinicipal from token.</returns>
        public ClaimsPrincipal? GetPrincipalFromToken(string? token);

        /// <summary>
        /// Creates token basing on auth claims.
        /// </summary>
        /// <param name="authClaims">List of claims.</param>
        /// <returns>Created token.</returns>
        JwtSecurityToken CreateToken(List<Claim> authClaims);

        /// <summary>
        /// Refreshes given token.
        /// </summary>
        /// <param name="token">Token of user to be refreshed.</param>
        /// <returns>Refreshed token if everything is correct, null otherwise.</returns>
        public Task<Token?> RefreshToken(Token token);
    }
}
