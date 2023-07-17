using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;
using ToDo.DomainModel.Classes;
using ToDo.DomainModel.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IToDoListService"/> interface.
    /// </summary>
    public class ToDoListService : IToDoListService
    {
        private readonly IRepository<ToDoList> listRepository;
        private readonly IRepository<ToDoItem> itemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        public ToDoListService(IRepository<ToDoList> listRepository, IRepository<ToDoItem> itemRepository)
        {
            this.listRepository = listRepository;
            this.itemRepository = itemRepository;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        public async Task<ToDoList?> AddList(ToDoList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "list must not be null");
            }

            await listRepository.Insert(list);

            return list;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="ToDoList"/> in database.</exception>
        public async Task<ToDoListStatistics> CopyList(int id)
        {
            var list = await listRepository.GetByID(id) ??
                throw new ArgumentException("there is no such list");

            var newList = await listRepository.Insert(new ToDoList
            {
                Title = list.Title + " (Copy)",
                IsArchived = list.IsArchived,
            });

            var items = list.Items!.ToList();

            if (items.Any())
            {
                foreach (var item in items)
                {

                    await itemRepository.Insert(new ToDoItem
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Deadline = item.Deadline,
                        Status = item.Status,
                        CreatedAt = item.CreatedAt,
                        Remind = item.Remind,
                        ToDoListID = newList.Id,
                        TodoList = newList,
                        Priority = item.Priority,
                    });
                }
            }

            return new ToDoListStatistics
            {
                Id = newList.Id,
                Title = newList.Title,
                IsArchived = newList.IsArchived,
                ItemsCompleted = items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            };
        }

        /// <inheritdoc/>
        public void DeleteList(int id)
        {
            listRepository.Delete(id);
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetArchivedLists()
        {
            return listRepository.GetAll().Where(l => l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<ToDoList?> GetListByID(int id)
        {
            return await this.listRepository.GetByID(id);
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetNotArchivedLists()
        {
            var lists = listRepository.GetAll();
            return lists.Where(l => !l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            }).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetAllLists()
        {
            var lists = listRepository.GetAll();
            return lists.Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            }).ToList();
        }

        /// <inheritdoc/>
        public ToDoList UpdateList(ToDoList list)
        {
            listRepository.Update(list);

            return list;
        }
    }
}
