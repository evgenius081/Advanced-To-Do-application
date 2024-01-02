using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="ToDoItem"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/items")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService toDoItemService;
        private readonly IToDoListService toDoListService;
        private readonly IHttpContextService httpContextService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> class.
        /// </summary>
        /// <param name="toDoItemService">Service for <see cref="ToDoItem"/>.</param>
        /// <param name="httpContextService">Service for http context.</param>
        /// <param name="toDoListService">Service for <see cref="ToDoList"/>.</param>
        /// <param name="logger">Logger.</param>
        public ToDoItemController(
            IToDoItemService toDoItemService,
            IHttpContextService httpContextService,
            IToDoListService toDoListService,
            ILogger<ToDoItemController> logger)
        {
            this.toDoItemService = toDoItemService;
            this.httpContextService = httpContextService;
            this.toDoListService = toDoListService;
            this.logger = logger;
        }

        /// <summary>
        /// Handles request for getting all <see cref="ToDoItem"/> with common <see cref="ToDoItem.ToDoListID"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoItem.ToDoListID"/> to be searched by.</param>
        /// <returns>NotFound response if there is no such <see cref="ToDoList.Id"/>, Ok response with <see cref="ToDoItem"/> with common <see cref="ToDoItem.ToDoListID"/> otherwise.</returns>
        [HttpGet]
        [Route("list/{id:int}")]
        public async Task<IActionResult> GetItemsByListID(int id)
        {
            var list = await this.toDoListService.GetListByID(id);

            if (list == null)
            {
                return this.NotFound("List not found");
            }

            if (list.UserID != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.BadRequest("This list does not belong to you.");
            }

            var listItems = await this.toDoItemService.GetItemsByListID(id);
            return this.Ok(listItems);
        }

        /// <summary>
        /// Handles request for getting todays items.
        /// </summary>
        /// <returns>Ok response with list of <see cref="ToDoItem"/> which <see cref="ToDoItem.Deadline"/> is due today.</returns>
        [HttpGet]
        [Route("today")]
        public IActionResult GetToday()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var todayItems = this.toDoItemService.GetItemsByDate(DateTime.Now.Date);
            return this.Ok(todayItems.Where(i => i.TodoList.UserID == userID));
        }

        /// <summary>
        /// Handles request for getting todays <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with list of <see cref="ToDoItem"/> which <see cref="ToDoItem.Deadline"/> is due today.</returns>
        [HttpGet]
        [Route("remind")]
        public ActionResult GetReminded()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var remindedItems = this.toDoItemService.GetItemsForReminder();
            return this.Ok(remindedItems.Where(i => i.TodoList.UserID == userID));
        }

        /// <summary>
        /// Handles request for getting all starred <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with all <see cref="ToDoItem"/> that have <see cref="ToDoItem.Starred"/> set.</returns>
        [HttpGet]
        [Route("primary")]
        public ActionResult GetStarred()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var starredItems = this.toDoItemService.GetItemsByPriority(Priority.High);
            return this.Ok(starredItems.Where(i => i.TodoList.UserID == userID));
        }

        /// <summary>
        /// Handles request for getting <see cref="ToDoItem"/> by specified <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoItem.Id"/> to be searched by.</param>
        /// <returns>NotFound response if there is no such <see cref="ToDoItem.Id"/>, Ok response with <see cref="ToDoItem"/> otherwise.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetItemByID(int id)
        {
            var item = await this.toDoItemService.GetItem(id);
            if (item == null)
            {
                return this.NotFound();
            }

            if (item.TodoList.UserID != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.BadRequest("This list does not belong to you.");
            }

            return this.Ok(item);
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoItem"/> by specified <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="item">Updated <see cref="ToDoItem"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoItem"/> is empty or <see cref="ToDoItem.Id"/> in query and <see cref="ToDoItem.Id"/> in <see cref="ToDoItem"/> (or <see cref="ToDoItem.Id"/> in found item and <see cref="ToDoItem"/> in sent <see cref="ToDoItem"/>) are different, NotFound response if there is no such <see cref="ToDoItem"/>, Ok response with the updated <see cref="ToDoItem"/>.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] ToDoItemUpdate item)
        {
            if ((await this.toDoListService.GetListByID(item.ToDoListID)).UserID != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.BadRequest("This list does not belong to you.");
            }

            try
            {
                var updatedItem = await this.toDoItemService.UpdateItem(item);
                return this.Ok(updatedItem);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentException)
            {
                return this.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request for InsertAsyncing <see cref="ToDoItem"/>.
        /// </summary>
        /// <param name="item"><see cref="ToDoItem"/> to be InsertAsynced.</param>
        /// <returns>BadRequest response incase passed <see cref="ToDoItem"/> is empty or it is assigned to unexisting <see cref="ToDoList"/> or Created response with created <see cref="ToDoItem"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ToDoItemCreate item)
        {
            if ((await this.toDoListService.GetListByID(item.ToDoListID)).UserID != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.BadRequest("This list does not belong to you.");
            }

            try
            {
                var createdItem = await this.toDoItemService.AddItem(item);
                return this.Created("todolists", createdItem);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request for deleting <see cref="ToDoItem"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoItem.Id"/> to be searched by.</param>
        /// <returns>NotFound response in case there is no such <see cref="ToDoItem"/> or Accepted response in case it was successfully deleted.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            this.logger.LogInformation($"deleted item with id {id}");
            if ((await this.toDoItemService.GetItem(id)).TodoList.UserID != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.BadRequest("This list does not belong to you.");
            }

            try
            {
                this.toDoItemService.DeleteItem(id);
                return this.Accepted();
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }
    }
}
