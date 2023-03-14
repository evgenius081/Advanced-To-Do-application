using System;
using System.Collections.Generic;
using System.Text;

namespace TODOListDomainModel.Classes
{
    /// <summary>
    /// Represents possible statuses of <see cref="ToDoItem" />.
    /// </summary>
    public enum Status
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
