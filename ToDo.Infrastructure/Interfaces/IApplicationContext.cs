using System;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Models;

namespace ToDo.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for ApplicationContext.
    /// </summary>
    public interface IApplicationContext : IDisposable
    {
        /// <summary>
        /// Gets or sets set of <see cref="ToDoItem"/> in database.
        /// </summary>
        DbSet<ToDoItem> Items { get; set; }

        /// <summary>
        /// Gets or sets set of <see cref="ToDoList"/> in database.
        /// </summary>
        DbSet<ToDoList> Lists { get; set; }

        /// <summary>
        /// Gets or sets set of <see cref="User"/> in database.
        /// </summary>
        DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets set of <see cref="Notification"/> in database.
        /// </summary>
        DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Saves changes in database.
        /// </summary>
        /// <returns>Number of written entries into database.</returns>
        int SaveChanges();
    }
}
