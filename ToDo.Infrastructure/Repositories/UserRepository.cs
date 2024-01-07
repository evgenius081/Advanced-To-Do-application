using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Infrastructure.Interfaces;

namespace ToDo.Infrastructure.Repositories
{
    /// <summary>
    /// Class implementing <see cref="IUserRepository"/>.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context"><see cref="IApplicationContext"/> object.</param>
        /// <exception cref="ArgumentNullException">Thrown if context or is null.</exception>
        public UserRepository(IApplicationContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context), "Context object must not be null.");
        }

        /// <inheritdoc />
        public IEnumerable<User> GetAll()
        {
            return this.context.Users;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="User"/> in database.</exception>
        public void Delete(int id)
        {
            var user = this.context.Users.SingleOrDefault(l => l.Id == id) ??
                throw new ArgumentException("There is no such user in database.");

            this.context.Users.Remove(user);
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        public async Task<User?> GetByID(int id)
        {
            return await this.context.Users.Include(u => u.Lists).SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="User"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if this <see cref="User"/> is already in database.</exception>
        public Task<User> InsertAsync(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user), "ToDo list object must not be null.");

            if (this.context.Users.Contains(user))
            {
                throw new ArgumentException("User already in base.");
            }

            return this.InsertAsyncAsync(user);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if <see cref="User"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="User"/> in database.</exception>
        public void Update(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user), "User object must not be null.");

            var userFound = this.context.Users.SingleOrDefault(t => t.Id == user.Id) ?? throw new ArgumentException("There is no such ToDoList in database.");

            userFound.Username = user.Username;
            userFound.PasswordHash = user.PasswordHash;
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await this.context.Users.Include(u => u.Lists).SingleOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// InsertAsyncs <see cref="User"/> into database, after InsertAsync function checks the object.
        /// </summary>
        /// <param name="user"><see cref="User"/> object ot be InsertAsynced into database.</param>
        /// <returns>Void async function result.</returns>
        private async Task<User> InsertAsyncAsync(User user)
        {
            await this.context.Users.AddAsync(user);
            this.context.SaveChanges();
            return user;
        }
    }
}
