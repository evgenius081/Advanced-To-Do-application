using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Classes;
using ToDo.Infrastructure.Interfaces;

namespace ToDo.Infrastructure.Context
{
    /// <summary>
    /// Application context class.
    /// </summary>
    public class ApplicationContext : DbContext, IApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">Options for Database Context.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets set of <see cref="ToDoItem"/>.
        /// </summary>
        public virtual DbSet<ToDoItem> Items { get; set; }

        /// <summary>
        /// Gets or sets set of <see cref="ToDoList"/>.
        /// </summary>
        public virtual DbSet<ToDoList> ToDoLists { get; set; }

        /// <summary>
        /// Sets relations between entities and seeds data.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>().HasMany<ToDoItem>(l => l.Items).WithOne(i => i.TodoList);
        }
    }
}
