using System.Threading.Tasks;
using ToDo.Services.DTOs;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Service for account.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Logins user in.
        /// </summary>
        /// <param name="userLogin">DTO with username and password.</param>
        /// <returns>Token if user is logged in successfully, empty string otherwise.</returns>
        Task<string?> Login(UserLogin userLogin);

        /// <summary>
        /// Registers user.
        /// </summary>
        /// <param name="userLogin">DTO with username and password.</param>
        /// <returns>Token if user is created and logged in successfully, empty string otherwise.</returns>
        Task<string?> Register(UserLogin userLogin);

        /// <summary>
        /// Logs user out.
        /// </summary>
        /// <param name="username">Login of a user to be logged out.</param>
        /// <returns>Async void.</returns>
        Task Logout(string username);
    }
}
