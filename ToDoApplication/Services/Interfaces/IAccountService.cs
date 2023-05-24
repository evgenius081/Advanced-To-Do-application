using System.Threading.Tasks;
using ToDoApplication.DTOs;

namespace ToDoApplication.Services.Interfaces
{
    /// <summary>
    /// Service for account.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Logins user in.
        /// </summary>
        /// <param name="userLogin">DTO with username an dpassword.</param>
        /// <returns>Token if user is logged in successfully, empty string otherwise.</returns>
        Task<string> Login(UserLogin userLogin);

        /// <summary>
        /// Logs user out.
        /// </summary>
        /// <param name="username">Login of a user to be logged out.</param>
        /// <returns>Async void.</returns>
        Task Logout(string username);
    }
}
