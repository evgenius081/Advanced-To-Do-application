using System.Threading.Tasks;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Interfaces
{
    /// <summary>
    /// Repository for <see cref="User"/> objects.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets user by username from database.
        /// </summary>
        /// <param name="username">Wanted user's username.</param>
        /// <returns>Found user, null if there is no such.</returns>
        public Task<User?> GetByUsernameAsync(string username);
    }
}
