using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Interfaces;
using ToDo.DomainModel.Models;
using ToDo.DomainModel.Models.NotificationData;
using ToDo.Services.DTOs;
using ToDo.Services.Jobs;
using ToDo.Services.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Service implementing <see cref="IToDoItemService"/> interface.
    /// </summary>
    public class ToDoItemService : IToDoItemService
    {
        private readonly IRepository<ToDoItem> itemRepository;
        private readonly IRepository<ToDoList> listRepository;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemService"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        /// <param name="configuration">Configuration.</param>
        public ToDoItemService(
            IRepository<ToDoItem> itemRepository,
            IRepository<ToDoList> listRepository,
            IConfiguration configuration)
        {
            this.itemRepository = itemRepository;
            this.listRepository = listRepository;
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public List<ToDoItem> GetAll()
        {
            return this.itemRepository.GetAll().ToList();
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

            if (CheckIfRemind(item))
            {
                await this.CreateNotification(item, list);
            }

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
            return this.itemRepository.GetAll().Where(i => CheckIfRemind(i)).ToList();
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

            if (CheckIfRemind(foundItem))
            {
                await this.CreateNotification(foundItem, foundItem.TodoList);
            }

            return foundItem;
        }

        /// <summary>
        /// Checks if item has to be reminded now.
        /// </summary>
        /// <param name="item">Item to check for reminding.</param>
        /// <returns>True/false.</returns>
        private static bool CheckIfRemind(ToDoItem item)
        {
            var diff = item.Deadline.Subtract(DateTime.UtcNow).TotalMinutes;

            return item.Remind &&
            diff <= 90 &&
            diff >= 0 &&
            item.Status != ItemStatus.Completed;
        }

        /// <summary>
        /// Creates job for sending notification.
        /// </summary>
        /// <param name="item">Item to be reminded.</param>
        /// <param name="list">List the item belongs to.</param>
        /// <returns>Async void.</returns>
        private async Task CreateNotification(ToDoItem item, ToDoList? list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var schedulerFactory = new StdSchedulerFactory();
            schedulerFactory.Initialize();
            var scheduler = await schedulerFactory.GetScheduler(this.configuration.GetSection("Quartz:SchedulerId").Value!);
            var notificationData = new ReminderNotificationData
            {
                ToDoItemId = item.Id,
                Deadline = item.Deadline,
                ToDoListId = list.Id,
                ToDoItemName = item.Title,
                ToDoListName = list.Title,
            };
            var notification = new Notification
            {
                NotificationData = notificationData,
                NotificationState = NotificationState.Created,
                NotificationType = NotificationType.ReminderNotificationType,
                RecipientId = list.UserID,
                SentAt = DateTime.UtcNow,
            };
            var partialNotificationJobData = new JobDataMap { { "notification", JsonConvert.SerializeObject(notification, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) } };
            var sendNotificationJobKey = new JobKey(Guid.NewGuid().ToString());
            var job = JobBuilder.Create<SendItemNotificationJob>()
                .WithIdentity(sendNotificationJobKey)
                .UsingJobData(partialNotificationJobData)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString())
                .ForJob(sendNotificationJobKey)
                .StartAt(DateTime.UtcNow)
                .Build();
            await scheduler!.ScheduleJob(job, trigger);
        }
    }
}
