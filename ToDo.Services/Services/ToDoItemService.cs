using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

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
        public async Task<ToDoItem?> AddItem(ToDoItemCreate dto)
        {
            var list = await this.listRepository.GetByID(dto.ToDoListID);
            _ = list ?? throw new InvalidOperationException("There is no such list");

            var item = new ToDoItem()
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt,
                Deadline = dto.Deadline,
                Priority = dto.Priority,
                Remind = dto.Remind,
                Status = dto.Status,
                ToDoListID = dto.ToDoListID,
                TodoList = list,
            };

            return await this.itemRepository.InsertAsync(item);
        }

        /// <inheritdoc/>
        public void DeleteItem(int itemID)
        {
            this.itemRepository.Delete(itemID);
        }

        /// <inheritdoc/>
        public async Task<ToDoItem?> GetItem(int intemID)
        {
            return await this.itemRepository.GetByID(intemID);
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsByDate(DateTime date)
        {
            return this.itemRepository.GetAll().Where(i => i.Deadline.Date == date).ToList();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such list in database.</exception>
        public async Task<List<ToDoItem>> GetItemsByListID(int listID)
        {
            _ = await this.listRepository.GetByID(listID) ??
                throw new ArgumentException("There is no such list");

            return this.itemRepository.GetAll().Where(i => i.ToDoListID == listID).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsByPriority(Priority priority)
        {
            return this.itemRepository.GetAll().Where(item => item.Priority == priority).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoItem> GetItemsForReminder()
        {
            return this.itemRepository.GetAll().Where(i =>
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
        public async Task<ToDoItem?> UpdateItem(ToDoItemUpdate dto)
        {
            var foundItem = await this.itemRepository.GetByID(dto.Id) ??
                throw new ArgumentException("there is no such item");

            if (foundItem.ToDoListID != dto.ToDoListID)
            {
                throw new InvalidOperationException("you cannot update ToDoList this item is assigned to");
            }

            foundItem.Title = dto.Title;
            foundItem.Description = dto.Description;
            foundItem.Remind = dto.Remind;
            foundItem.Status = dto.Status;
            foundItem.CreatedAt = dto.CreatedAt;
            foundItem.Deadline = dto.Deadline;
            foundItem.Status = dto.Status;
            foundItem.Priority = dto.Priority;

            this.itemRepository.Update(foundItem);

            return foundItem;
        }
    }
}
