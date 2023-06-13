using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Classes;
using ToDo.DomainModel.Context;
using ToDo.DomainModel.Interfaces;

namespace ToDo.DomainModel.Repositories
{
    /// <summary>
    /// Repository for <see cref="ToDoList"/> object in database.
    /// </summary>
    public class ToDoListRepository : IRepository<ToDoList>
    {
        private readonly IApplicationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListRepository"/> class.
        /// </summary>
        /// <param name="context"><see cref="IApplicationContext"/> object.</param>
        /// <exception cref="ArgumentNullException">Thrown if context or is null.</exception>
        public ToDoListRepository(IApplicationContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context), "Context object must not be null.");
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="ToDoList"/> in database.</exception>
        public void Delete(int id)
        {
            var todo = this.context.ToDoLists.SingleOrDefault(l => l.Id == id) ??
                throw new ArgumentException("There is no such ToDo List in database.");

            this.context.ToDoLists.Remove(todo);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoList> GetAll()
        {
            return this.context.ToDoLists.Include(l => l.Items);
        }

        /// <inheritdoc/>
        public async Task<ToDoList> GetByID(int id)
        {
            return await this.context.ToDoLists.SingleOrDefaultAsync(l => l.Id == id);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="ToDoList"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="ToDoList"/> in database.</exception>
        public void Update(ToDoList todo)
        {
            if (todo == null)
            {
                throw new ArgumentNullException(nameof(todo), "ToDo List object must not be null.");
            }

            var todoFound = this.context.ToDoLists.SingleOrDefault(t => t.Id == todo.Id);
            if (todoFound == null)
            {
                throw new ArgumentException("There is no such ToDoList in database.");
            }

            todoFound.Title = todo.Title;
            todoFound.IsArchived = todo.IsArchived;
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="ToDoList"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if this <see cref="ToDoList"/> is already in database.</exception>
        public Task<ToDoList> Insert(ToDoList todo)
        {
            if (todo == null)
            {
                throw new ArgumentNullException(nameof(todo), "ToDo list object must not be null.");
            }

            if (this.context.ToDoLists.Contains(todo))
            {
                throw new ArgumentException("ToDo list already in base.");
            }

            return this.InsertAsync(todo);
        }

        /// <summary>
        /// Inserts <see cref="ToDoList"/> into database, after Insert function checks the object.
        /// </summary>
        /// <param name="todo"><see cref="ToDoList"/> object ot be inserted into database.</param>
        /// <returns>Void async function result.</returns>
        private async Task<ToDoList> InsertAsync(ToDoList todo)
        {
            await this.context.ToDoLists.AddAsync(todo);
            this.context.SaveChanges();
            return todo;
        }
    }
}
