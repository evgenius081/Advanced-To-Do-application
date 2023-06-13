using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.DomainModel.Classes;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="ToDoItem"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("items")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService toDoItemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> class.
        /// </summary>
        /// <param name="toDoItemService">Service for <see cref="ToDoList"/>.</param>
        public ToDoItemController(IToDoItemService toDoItemService)
        {
            this.toDoItemService = toDoItemService;
        }

        /// <summary>
        /// Handles request for getting all <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoItem"/>.</returns>
        [HttpGet]
        public ActionResult GetAllItems()
        {
            var items = this.toDoItemService.GetAllItems();
            return this.Ok(items);
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
            var todayItems = this.toDoItemService.GetItemsByDate(DateTime.Now.Date);
            return this.Ok(todayItems);
        }

        /// <summary>
        /// Handles request for getting todays <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with list of <see cref="ToDoItem"/> which <see cref="ToDoItem.Deadline"/> is due today.</returns>
        [HttpGet]
        [Route("remind")]
        public ActionResult GetReminded()
        {
            var remindedItems = this.toDoItemService.GetItemsForReminder();
            return this.Ok(remindedItems);
        }

        /// <summary>
        /// Handles request for getting all starred <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with all <see cref="ToDoItem"/> that have <see cref="ToDoItem.Starred"/> set.</returns>
        [HttpGet]
        [Route("starred")]
        public ActionResult GetStarred()
        {
            var starredItems = this.toDoItemService.GetItemsByPriority(Priority.Top);
            return this.Ok(starredItems);
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

            return this.Ok(item);
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoItem"/> by specified <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="item">Updated <see cref="ToDoItem"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoItem"/> is empty or <see cref="ToDoItem.Id"/> in query and <see cref="ToDoItem.Id"/> in <see cref="ToDoItem"/> (or <see cref="ToDoItem.Id"/> in found item and <see cref="ToDoItem"/> in sent <see cref="ToDoItem"/>) are different, NotFound response if there is no such <see cref="ToDoItem"/>, Ok response with the updated <see cref="ToDoItem"/>.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] ToDoItem item)
        {
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
        /// Handles request for inserting <see cref="ToDoItem"/>.
        /// </summary>
        /// <param name="item"><see cref="ToDoItem"/> to be inserted.</param>
        /// <returns>BadRequest response incase passed <see cref="ToDoItem"/> is empty or it is assigned to unexisting <see cref="ToDoList"/> or Created response with created <see cref="ToDoItem"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ToDoItem item)
        {
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
        public IActionResult DeleteItem(int id)
        {
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
