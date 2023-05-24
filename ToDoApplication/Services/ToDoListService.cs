using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApplication.DTOs;
using ToDoApplication.Services.Interfaces;
using TODOListDomainModel.Classes;
using TODOListDomainModel.Interfaces;

namespace ToDoApplication.Services
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
        public async Task<ToDoList> AddList(ToDoList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "list must not be null");
            }

            await this.listRepository.Insert(list);

            return list;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if there is no such <see cref="ToDoList"/> in database.</exception>
        public async Task<ToDoList> CopyList(int id)
        {
            var list = await this.listRepository.GetByID(id) ??
                throw new ArgumentException("there is no such list");

            var newList = await this.listRepository.Insert(new ToDoList
            {
                Title = list.Title + " (Copy)",
                IsArchived = list.IsArchived,
            });
            newList.Items = new List<ToDoItem>();

            var items = list.Items.ToList();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    await this.itemRepository.Insert(new ToDoItem
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

            return newList;
        }

        /// <inheritdoc/>
        public void DeleteList(int id)
        {
            this.listRepository.Delete(id);
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetArchivedLists()
        {
            var lists = this.listRepository.GetAll();
            return lists.Where(l => l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<ToDoList> GetListByID(int id)
        {
            return await this.listRepository.GetByID(id);
        }

        /// <inheritdoc/>
        public List<ToDoListStatistics> GetNotArchivedLists()
        {
            return this.listRepository.GetAll().Where(l => !l.IsArchived).Select(l => new ToDoListStatistics
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                ItemsCompleted = l.Items.Where(i => i.Status == ItemStatus.Completed).Count(),
                ItemsInProcess = l.Items.Where(i => i.Status == ItemStatus.InProcess).Count(),
                ItemsNotStarted = l.Items.Where(i => i.Status == ItemStatus.NotStarted).Count(),
            }).ToList();
        }

        /// <inheritdoc/>
        public ToDoList UpdateList(ToDoList list)
        {
            this.listRepository.Update(list);

            return list;
        }
    }
}
