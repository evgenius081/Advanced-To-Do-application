using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;

namespace ToDo.Services.Services.Interfaces
{
    /// <summary>
    /// Service for <see cref="ToDoItem"/>.
    /// </summary>
    public interface IToDoItemService
    {
        /// <summary>
        /// Gets list of all <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>List of all <see cref="ToDoItem"/>.</returns>
        List<ToDoItem> GetAll();

        /// <summary>
        /// Gets <see cref="ToDoItem"/> by <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="listID">Id to be sarched by.</param>
        /// <returns>Found item, null otherwise.</returns>
        Task<List<ToDoItem>> GetItemsByListID(int listID);

        /// <summary>
        /// Gets list of <see cref="ToDoItem"/> by <see cref="ToDoItem.Deadline"/>.
        /// </summary>
        /// <param name="date">Date to be searched by.</param>
        /// <returns>List of items with specified deadline.</returns>
        List<ToDoItem> GetItemsByDate(DateTime date);

        /// <summary>
        /// Gets list of <see cref="ToDoItem"/> by <see cref="ToDoItem.Priority"/>.
        /// </summary>
        /// <param name="priority">Priority level to be searched by.</param>
        /// <returns>List of items with specified priority.</returns>
        List<ToDoItem> GetItemsByPriority(Priority priority);

        /// <summary>
        /// Gets items with <see cref="ToDoItem.Remind" /> set to true.
        /// </summary>
        /// <returns>List of items.</returns>
        List<ToDoItem> GetItemsForReminder();

        /// <summary>
        /// Gets <see cref="ToDoItem"/> by <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="itemID">Id to be searched by.</param>
        /// <returns>Found item, null otherwise.</returns>
        Task<ToDoItem?> GetItem(int itemID);

        /// <summary>
        /// Updates <see cref="ToDoItem"/> in database.
        /// </summary>
        /// <param name="dto">Item to be updated.</param>
        /// <returns>Updated item.</returns>
        Task<ToDoItem?> UpdateItem(ToDoItemUpdate dto);

        /// <summary>
        /// Adds <see cref="ToDoItem"/> to database.
        /// </summary>
        /// <param name="dto">Item to be updated.</param>
        /// <returns>Added item with all fields.</returns>
        Task<ToDoItem?> AddItem(ToDoItemCreate dto);

        /// <summary>
        /// Deletes <see cref="ToDoItem"/> from database.
        /// </summary>
        /// <param name="itemID">Id to be deleted by.</param>
        void DeleteItem(int itemID);
    }
}
