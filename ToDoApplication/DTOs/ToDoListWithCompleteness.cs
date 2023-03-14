using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TODOListDomainModel.Classes;

namespace ToDoApplication.DTOs
{
    /// <summary>
    /// DTO for <see cref="ToDoList"/>.
    /// </summary>
    public class ToDoListWithCompleteness
    {
        /// <summary>
        /// Gets or sets <see cref="ToDoList.Id"/>.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ToDoList.Title"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="ToDoList.IsArchived"/>.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets amount of <see cref="ToDoItem"/> that belong to this <see cref="ToDoList"/> and have <see cref="Status.InProcess"/> as their <see cref="ToDoItem.Status"/>.
        /// </summary>
        public int InProcess { get; set; }

        /// <summary>
        /// Gets or sets amount of <see cref="ToDoItem"/> that belong to this <see cref="ToDoList"/> and have <see cref="Status.NotStarted"/> as their <see cref="ToDoItem.Status"/>.
        /// </summary>
        public int NotStarted { get; set; }

        /// <summary>
        /// Gets or sets amount of <see cref="ToDoItem"/> that belong to this <see cref="ToDoList"/> and have <see cref="Status.Completed"/> as their <see cref="ToDoItem.Status"/>.
        /// </summary>
        public int Completed { get; set; }
    }
}
