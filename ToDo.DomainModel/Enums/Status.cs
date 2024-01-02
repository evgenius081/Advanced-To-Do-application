using System;
using System.Collections.Generic;
using System.Text;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Enums
{
    /// <summary>
    /// Represents possible statuses of <see cref="ToDoItem" />.
    /// </summary>
    public enum ItemStatus
    {
        /// <summary>
        /// ToDo item was not started.
        /// </summary>
        NotStarted,

        /// <summary>
        /// ToDo item is in process.
        /// </summary>
        InProcess,

        /// <summary>
        /// ToDo item is completed.
        /// </summary>
        Completed,
    }
}
