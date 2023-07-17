using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Services.DTOs;
using ToDo.DomainModel.Classes;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Interface for service managing <see cref="ToDoList" />.
    /// </summary>
    public interface IToDoListService
    {
        /// <summary>
        /// Gets all not archived lists.
        /// </summary>
        /// <returns>List of <see cref="ToDoList"/> with <see cref="ToDoList.IsArchived" /> set to false.</returns>
        List<ToDoListStatistics> GetNotArchivedLists();

        /// <summary>
        /// Gets all archived lists.
        /// </summary>
        /// <returns>List of <see cref="ToDoList"/> with <see cref="ToDoList.IsArchived" /> set to true.</returns>
        List<ToDoListStatistics> GetArchivedLists();

        /// <summary>
        /// Gets all lists.
        /// </summary>
        /// <returns>List of all <see cref="ToDoList"/>.</returns>
        List<ToDoListStatistics> GetAllLists();

        /// <summary>
        /// Gets list by ID.
        /// </summary>
        /// <param name="listID">Id of list to be taken.</param>
        /// <returns>List if exists, null otherwise.</returns>
        Task<ToDoList?> GetListByID(int listID);

        /// <summary>
        /// Creates a copy of a list with all its items.
        /// </summary>
        /// <param name="listID">Id of a list to be copied.</param>
        /// <returns>Copied list.</returns>
        Task<ToDoListStatistics> CopyList(int listID);

        /// <summary>
        /// Updates given list in database.
        /// </summary>
        /// <param name="list">List to update.</param>
        /// <returns>Updated list.</returns>
        ToDoList UpdateList(ToDoList list);

        /// <summary>
        /// Inserts list to database.
        /// </summary>
        /// <param name="list">List to insert.</param>
        /// <returns>Inserted list.</returns>
        Task<ToDoList?> AddList(ToDoList list);

        /// <summary>
        /// Deleted list from database.
        /// </summary>
        /// <param name="listID">List's id to delete.</param>
        void DeleteList(int listID);
    }
}
