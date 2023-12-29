using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.DomainModel.Models
{
    /// <summary>
    /// Represents possible priorities for <see cref="ToDoItem"/>.
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// Item has standard priority.
        /// </summary>
        Standard,

        /// <summary>
        /// Item has maximum priority.
        /// </summary>
        High,
    }
}
