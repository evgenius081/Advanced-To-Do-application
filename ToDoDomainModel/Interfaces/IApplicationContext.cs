using System;
using Microsoft.EntityFrameworkCore;
using TODOListDomainModel.Classes;

namespace TODOListDomainModel.Interfaces
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
        DbSet<ToDoList> ToDoLists { get; set; }

        /// <summary>
        /// Saves changes in database.
        /// </summary>
        /// <returns>Number of written entries into database.</returns>
        int SaveChanges();
    }
}
