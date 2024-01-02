using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IToDoListService"/> interface.
    /// </summary>
    public class ToDoListService : IToDoListService
    {
        private readonly IRepository<ToDoList> listRepository;
        private readonly IRepository<ToDoItem> itemRepository;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        /// <param name="userRepository">Repository for <see cref="User"/>.</param>
        public ToDoListService(IRepository<ToDoList> listRepository, IRepository<ToDoItem> itemRepository, IUserRepository userRepository)
        {
            this.listRepository = listRepository;
            this.itemRepository = itemRepository;
            this.userRepository = userRepository;
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetArchivedListsWithDetails()
        {
            return this.listRepository.GetAll().Where(l => l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
                UserID = l.UserID,
            }).ToList();
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetNotArchivedListsWithDetails()
        {
            var lists = this.listRepository.GetAll();
            return lists.Where(l => !l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
                UserID = l.UserID,
            }).ToList();
        }

        /// <inheritdoc />
        public List<ToDoListStatistics> GetAllListsWithDetails()
        {
            var lists = this.listRepository.GetAll();
            return lists.Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items!.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items!.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items!.Where(i => i.Status == ItemStatus.NotStarted).Count(),
                UserID = l.UserID,
            }).ToList();
        }

        /// <inheritdoc />
        public List<ToDoList> GetNotArchivedLists()
        {
            return this.listRepository.GetAll().Where(list => !list.IsArchived).ToList();
        }

        /// <inheritdoc />
        public List<ToDoList> GetArchivedLists()
        {
            return this.listRepository.GetAll().Where(list => list.IsArchived).ToList();
        }

        /// <inheritdoc />
        public List<ToDoList> GetAllLists()
        {
            return this.listRepository.GetAll().ToList();
        }

        /// <inheritdoc/>
        public async Task<ToDoList?> GetListByID(int id)
        {
            return await this.listRepository.GetByID(id);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        public async Task<ToDoList?> AddList(ToDoListCreate dto)
        {
            _ = dto ?? throw new ArgumentNullException(nameof(dto), "dto must not be null");

            var user = await this.userRepository.GetByID(dto.UserID) ?? throw new ArgumentException("There is no such user");

            var list = new ToDoList()
            {
                Title = dto.Title,
                IsArchived = dto.IsArchived,
                UserID = dto.UserID,
                User = user,
            };

            await this.listRepository.InsertAsync(list);

            return list;
        }

        /// <inheritdoc/>
        public async Task<ToDoList> UpdateList(ToDoListUpdate dto)
        {
            _ = dto ?? throw new ArgumentNullException(nameof(dto));

            var list = await this.listRepository.GetByID(dto.Id) ?? throw new ArgumentException("there is no such list");

            list.Title = dto.Title;
            list.IsArchived = dto.IsArchived;

            this.listRepository.Update(list);

            return list;
        }

        /// <inheritdoc/>
        public void DeleteList(int id)
        {
            this.listRepository.Delete(id);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="ToDoList"/> in database.</exception>
        public async Task<ToDoListStatistics> CopyList(int id)
        {
            var list = await this.listRepository.GetByID(id) ??
                throw new ArgumentException("there is no such list");

            var newList = await this.listRepository.InsertAsync(new ToDoList
            {
                Title = list.Title + " (Copy)",
                IsArchived = list.IsArchived,
                User = list.User,
                UserID = list.UserID,
            });

            var items = list.Items!.ToList();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    await this.itemRepository.InsertAsync(new ToDoItem
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
                UserID = newList.UserID,
            };
        }
    }
}
