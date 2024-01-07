﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.Services.DTOs;
using ToDo.Services.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests about tokens.
    /// </summary>
    [Route("api/tokens")]
    public class TokenController : Controller
    {
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="tokenService">Service for handling tokens.</param>
        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Refresehs given token using refresh token.
        /// </summary>
        /// <param name="token">Object containing token and refresh token.</param>
        /// <returns>Refreshed token.</returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token token)
        {
            var refreshedToken = await this.tokenService.RefreshToken(token);

            if (refreshedToken == null)
            {
                return this.BadRequest("Token is invalid.");
            }

            return this.Ok(refreshedToken);
        }
    }
}
