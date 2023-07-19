﻿using System;
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
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">Service for signing in/out user.</param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
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
            var tokenString = await this.userService.Login(bodyUser);
            if (!string.IsNullOrEmpty(tokenString))
            {
                return this.Ok(tokenString);
            }

            return this.BadRequest("Wrong username or password");
        }

        /// <summary>
        /// Handles request for signing user in.
        /// </summary>
        /// <param name="bodyUser">User login data.</param>
        /// <returns>Ok response if login and password are valid, BadRequest if login, password or model in total is wrong.</returns>
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserLogin bodyUser)
        {
            var tokenString = await this.userService.Register(bodyUser);
            try
            {
                if (!string.IsNullOrEmpty(tokenString))
                {
                    return this.Ok(tokenString);
                }

                return this.BadRequest("User with this username already exists");
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest($"{ex.Message}");
            }
        }
    }
}