using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TODOListDomainModel.Classes;
using TODOListDomainModel.Context;
using TODOListDomainModel.Interfaces;

namespace ToDoDomainModel.Tests
{
    /// <summary>
    /// Mock for <see cref="ApplicationContext"/> for testing purposes.
    /// </summary>
    public class FakeApplicationContext : DbContext, IApplicationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeApplicationContext"/> class.
        /// </summary>
        /// <param name="options">Options for Database Context.</param>
        public FakeApplicationContext(DbContextOptions<FakeApplicationContext> options)
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
    }
}
