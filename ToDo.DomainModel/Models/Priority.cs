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
        /// Item is hidden.
        /// </summary>
        Low,

        /// <summary>
        /// Default status.
        /// </summary>
        Default,

        /// <summary>
        /// Item is starred.
        /// </summary>
        Top,
    }
}
