using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ToDo.Services.Interfaces;
using ToDo.DomainModel.Classes;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Repositories;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IToDoItemService"/> interface.
    /// </summary>
    public class ToDoItemService : IToDoItemService
    {
        private readonly IRepository<ToDoItem> itemRepository;
        private readonly IRepository<ToDoList> listRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemService"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        public ToDoItemService(IRepository<ToDoItem> itemRepository, IRepository<ToDoList> listRepository)
        {
            this.itemRepository = itemRepository;
            this.listRepository = listRepository;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ToDoList"/> item is assigned to does not exist.</exception>
        public async Task<ToDoItem> AddItem(ToDoItem item)
        {
            var list = await listRepository.GetByID(item.ToDoListID);
            item.TodoList = list ?? throw new InvalidOperationException("There is no such list");

            await itemRepository.Insert(item);

            return item;
        }

        /// <inheritdoc/>
        public void DeleteItem(int itemID)
        {
            itemRepository.Delete(itemID);
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetAllItems()
        {
            return itemRepository.GetAll().ToList();
        }

        /// <inheritdoc/>
        public async Task<ToDoItem> GetItem(int intemID)
        {
            return await itemRepository.GetByID(intemID);
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsByDate(DateTime date)
        {
            return itemRepository.GetAll().Where(i => i.Deadline.Date == date).ToList();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such list in database.</exception>
        public async Task<List<ToDoItem>> GetItemsByListID(int listID)
        {
            _ = await listRepository.GetByID(listID) ??
                throw new ArgumentException("There is no such list");

            return itemRepository.GetAll().Where(i => i.ToDoListID == listID).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsByPriority(Priority priority)
        {
            return itemRepository.GetAll().Where(item => item.Priority == priority).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsForReminder()
        {
            return itemRepository.GetAll().Where(i =>
            i.Deadline.Subtract(DateTime.Now).TotalMinutes <= 60 &&
            i.Deadline.Subtract(DateTime.Now).TotalMinutes >= 0 &&
            i.Status != ItemStatus.Completed &&
            i.Remind).ToList();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if passed item is null.</exception>
        /// <exception cref="ArgumentException">Thrown if item does not exist in database.</exception>
        /// <exception cref="InvalidOperationException">Thrown on attempt to change <see cref="ToDoList"/> this item is assigned
        ///  to.</exception>
        public async Task<ToDoItem> UpdateItem(ToDoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "item cannot be null");
            }

            var foundItem = await itemRepository.GetByID(item.Id) ??
                throw new ArgumentException("there is no such item");

            if (foundItem.ToDoListID != item.ToDoListID)
            {
                throw new InvalidOperationException("you cannot update ToDoList this item is assigned to");
            }

            item.TodoList = foundItem.TodoList;

            itemRepository.Update(item);

            return item;
        }
    }
}
