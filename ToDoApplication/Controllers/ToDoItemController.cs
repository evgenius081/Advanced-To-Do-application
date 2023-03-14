using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOListDomainModel.Classes;
using TODOListDomainModel.Interfaces;

namespace ToDoApplication.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="ToDoItem"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("items")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IRepository<ToDoItem> itemRepository;
        private readonly IRepository<ToDoList> listRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemController"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        public ToDoItemController(IRepository<ToDoItem> itemRepository, IRepository<ToDoList> listRepository)
        {
            this.itemRepository = itemRepository;
            this.listRepository = listRepository;
        }

        /// <summary>
        /// Handles request for getting all <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoItem"/>.</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllItems()
        {
            var items = await this.itemRepository.GetAll();
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
            var list = await this.listRepository.GetByID(id);

            if (list == null)
            {
                return this.NotFound();
            }

            var items = await this.itemRepository.GetAll();
            var listItems = items.Where(i => i.ToDoListID == id).ToList();
            return this.Ok(listItems);
        }

        /// <summary>
        /// Handles request for getting todays items.
        /// </summary>
        /// <returns>Ok response with list of <see cref="ToDoItem"/> which <see cref="ToDoItem.Deadline"/> is due today.</returns>
        [HttpGet]
        [Route("today")]
        public async Task<IActionResult> GetToday()
        {
            var items = await this.itemRepository.GetAll();
            var todayItems = items.Where(i => i.Deadline.Date == DateTime.Now.Date).ToList();

            return this.Ok(todayItems);
        }

        /// <summary>
        /// Handles request for getting todays <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with list of <see cref="ToDoItem"/> which <see cref="ToDoItem.Deadline"/> is due today.</returns>
        [HttpGet]
        [Route("remind")]
        public async Task<ActionResult> GetReminded()
        {
            var items = await this.itemRepository.GetAll();
            var remindedItems = items.Where(i =>
            (i.Deadline.Subtract(DateTime.Now).TotalMinutes <= 60) &&
            (i.Deadline.Subtract(DateTime.Now).TotalMinutes >= 0) &&
            (i.Status != Status.Completed) &&
            i.Remind).ToList();

            return this.Ok(remindedItems);
        }

        /// <summary>
        /// Handles request for getting all starred <see cref="ToDoItem"/>.
        /// </summary>
        /// <returns>Ok response with all <see cref="ToDoItem"/> that have <see cref="ToDoItem.Starred"/> set.</returns>
        [HttpGet]
        [Route("starred")]
        public async Task<ActionResult> GetStarred()
        {
            var items = await this.itemRepository.GetAll();

            return this.Ok(items.Where(item => item.Starred).ToList());
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
            var item = await this.itemRepository.GetByID(id);
            if (item == null)
            {
                return this.NotFound();
            }

            return this.Ok(item);
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoItem"/> by specified <see cref="ToDoItem.Id"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoItem.Id"/> to be searched by.</param>
        /// <param name="item">Updated <see cref="ToDoItem"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoItem"/> is empty or <see cref="ToDoItem.Id"/> in query and <see cref="ToDoItem.Id"/> in <see cref="ToDoItem"/> (or <see cref="ToDoItem.Id"/> in found item and <see cref="ToDoItem"/> in sent <see cref="ToDoItem"/>) are different, NotFound response if there is no such <see cref="ToDoItem"/>, Ok response with the updated <see cref="ToDoItem"/>.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditItem(int id, [FromBody] ToDoItem item)
        {
            if (item == null)
            {
                return this.BadRequest("Sent item object is empty.");
            }

            if (item.Id != id)
            {
                return this.BadRequest("Id in query and Id of object are different.");
            }

            var foundItem = await this.itemRepository.GetByID(id);

            if (foundItem == null)
            {
                return this.NotFound();
            }

            if (foundItem.ToDoListID != item.ToDoListID)
            {
                return this.BadRequest("ToDo item Id in real object and ToDo item Id of sent object are different.");
            }

            item.TodoList = foundItem.TodoList;

            this.itemRepository.Update(item);

            return this.Ok(item);
        }

        /// <summary>
        /// Handles request for inserting <see cref="ToDoItem"/>.
        /// </summary>
        /// <param name="item"><see cref="ToDoItem"/> to be inserted.</param>
        /// <returns>BadRequest response incase passed <see cref="ToDoItem"/> is empty or it is assigned to unexisting <see cref="ToDoList"/> or Created response with created <see cref="ToDoItem"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ToDoItem item)
        {
            if (item == null)
            {
                return this.BadRequest("Passed object is empty.");
            }

            var list = await this.listRepository.GetByID(item.ToDoListID);

            if (list == null)
            {
                return this.BadRequest("There is no ToDo list with this ID.");
            }

            item.TodoList = list;

            await this.itemRepository.Insert(item);

            return this.Created("todolists", item);
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
            var foundItem = await this.itemRepository.GetByID(id);

            if (foundItem == null)
            {
                return this.NotFound();
            }

            this.itemRepository.Delete(foundItem);

            return this.Accepted();
        }
    }
}
