using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.DomainModel.Classes
{
    /// <summary>
    /// Represents possible priorities for <see cref="ToDoItem"/>.
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// Default status.
        /// </summary>
        Default,

        /// <summary>
        /// Item is hidden.
        /// </summary>
        Low,

        /// <summary>
        /// Item is starred.
        /// </summary>
        Top,
    }
}
