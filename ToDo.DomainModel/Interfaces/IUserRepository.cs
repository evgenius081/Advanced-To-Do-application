using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Interfaces
{
    /// <summary>
    /// Repository for <see cref="User"/> objects.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// InsertAsync user to database.
        /// </summary>
        /// <param name="user">User object to be InsertAsynced into dataabse.</param>
        /// <returns>Task becuase method is async.</returns>
        public Task<User> InsertAsync(User user);

        /// <summary>
        /// Update user in databse.
        /// </summary>
        /// <param name="user">Object to be updated in database.</param>
        public void Update(User user);

        /// <summary>
        /// Delete user form database.
        /// </summary>
        /// <param name="id">Id of user to be deleted from database.</param>
        public void Delete(int id);

        /// <summary>
        /// Gets user by ID from database.
        /// </summary>
        /// <param name="id">Wanted user's ID.</param>
        /// <returns>Found user, null if there is no such.</returns>
        public Task<User?> GetByID(int id);

        /// <summary>
        /// Gets user by username from database.
        /// </summary>
        /// <param name="username">Wanted user's username.</param>
        /// <returns>Found user, null if there is no such.</returns>
        public Task<User?> GetByUsernameAsync(string username);
    }
}
