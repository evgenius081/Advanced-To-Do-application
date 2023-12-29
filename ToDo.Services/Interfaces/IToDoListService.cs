using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Interface for service managing <see cref="ToDoList" />.
    /// </summary>
    public interface IToDoListService
    {
        /// <summary>
        /// Gets all not archived lists with details.
        /// </summary>
        /// <returns>List of <see cref="ToDoListStatistics"/> with <see cref="ToDoList.IsArchived" /> set to false.</returns>
        List<ToDoListStatistics> GetNotArchivedListsWithDetails();

        /// <summary>
        /// Gets all archived lists with details.
        /// </summary>
        /// <returns>List of <see cref="ToDoListStatistics"/> with <see cref="ToDoList.IsArchived" /> set to true.</returns>
        List<ToDoListStatistics> GetArchivedListsWithDetails();

        /// <summary>
        /// Gets all lists with details.
        /// </summary>
        /// <returns>List of all <see cref="ToDoListStatistics"/>.</returns>
        List<ToDoListStatistics> GetAllListsWithDetails();

        /// <summary>
        /// Gets all not archived lists.
        /// </summary>
        /// <returns>List of <see cref="ToDoList"/> with <see cref="ToDoList.IsArchived" /> set to false.</returns>
        List<ToDoList> GetNotArchivedLists();

        /// <summary>
        /// Gets all archived lists.
        /// </summary>
        /// <returns>List of <see cref="ToDoList"/> with <see cref="ToDoList.IsArchived" /> set to true.</returns>
        List<ToDoList> GetArchivedLists();

        /// <summary>
        /// Gets all lists.
        /// </summary>
        /// <returns>List of all <see cref="ToDoList"/>.</returns>
        List<ToDoList> GetAllLists();

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
        /// <param name="dto">List to update.</param>
        /// <returns>Updated list.</returns>
        Task<ToDoList> UpdateList(ToDoListUpdate dto);

        /// <summary>
        /// InsertAsyncs list to database.
        /// </summary>
        /// <param name="dto">List to InsertAsync.</param>
        /// <returns>InsertAsynced list.</returns>
        Task<ToDoList?> AddList(ToDoListCreate dto);

        /// <summary>
        /// Deleted list from database.
        /// </summary>
        /// <param name="listID">List's id to delete.</param>
        void DeleteList(int listID);
    }
}
