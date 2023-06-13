using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.DomainModel.Classes;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="ToDoList"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("lists")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService toDoListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListController"/> class.
        /// </summary>
        /// <param name="toDoListService">Service for <see cref="ToDoList"/>.</param>
        public ToDoListController(IToDoListService toDoListService)
        {
            this.toDoListService = toDoListService;
        }

        /// <summary>
        /// Handles request for getting all not archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoList"/>.</returns>
        [HttpGet]
        public ActionResult GetNotArchivedLists()
        {
            var notArchivedLists = this.toDoListService.GetNotArchivedLists();
            return this.Ok(notArchivedLists);
        }

        /// <summary>
        /// Handles request for getting archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of archived <see cref="ToDoList"/>.</returns>
        [HttpGet]
        [Route("archived")]
        public ActionResult GetArchivedLists()
        {
            var archivedLists = this.toDoListService.GetArchivedLists();
            return this.Ok(archivedLists);
        }

        /// <summary>
        /// Handles request for getting <see cref="ToDoList"/> by specified <see cref="ToDoList.Id"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoList.Id"/> to be searched by.</param>
        /// <returns>NotFound response if there is no such <see cref="ToDoList.Id"/>, Ok response with <see cref="ToDoList"/> otherwise.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetListByID(int id)
        {
            var list = await this.toDoListService.GetListByID(id);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }

        /// <summary>
        /// handles request for copying existing <see cref="ToDoList"/> and its <see cref="ToDoItem"/> in database.
        /// </summary>
        /// <param name="id"><see cref="ToDoList.Id"/> to be searched by.</param>
        /// <returns>NotFound response if there is no such <see cref="ToDoList.Id"/>, Ok response with created <see cref="ToDoList"/> otherwise.</returns>
        [HttpGet]
        [Route("copy/{id:int}")]
        public async Task<IActionResult> CopyList(int id)
        {
            var newList = await this.CopyList(id);
            return this.Ok(newList);
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoList"/> by specified <see cref="ToDoList.Id"/>.
        /// </summary>
        /// <param name="list">Updated <see cref="ToDoList"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoList"/> is empty or <see cref="ToDoList.Id"/> in query and <see cref="ToDoList.Id"/> in object (or <see cref="ToDoList.Id"/> in found item and <see cref="ToDoList"/> in sent <see cref="ToDoList"/>) are different, NotFound response if there is no such <see cref="ToDoList"/>, Ok response with the updated <see cref="ToDoList"/>.</returns>
        [HttpPut]
        public IActionResult UpdateList([FromBody] ToDoList list)
        {
            if (this.ModelState.IsValid)
            {
                var updatedList = this.toDoListService.UpdateList(list);
                return this.Ok(updatedList);
            }

            return this.BadRequest("Invalid object provided.");
        }

        /// <summary>
        /// Handles request for inserting <see cref="ToDoList"/>.
        /// </summary>
        /// <param name="list"><see cref="ToDoList"/> to be inserted.</param>
        /// <returns>BadRequest response incase passed <see cref="ToDoList"/> is empty or Created response with created <see cref="ToDoList"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] ToDoList list)
        {
            var createdList = await this.toDoListService.AddList(list);
            return this.Created("todolists", createdList);
        }

        /// <summary>
        /// Handles request for deleting <see cref="ToDoList"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoList.Id"/> to be searched by.</param>
        /// <returns>NotFound response in case there is no such <see cref="ToDoList"/> or Accepted response in case it was successfully deleted.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteList(int id)
        {
            try
            {
                this.toDoListService.DeleteList(id);
                return this.Accepted();
            }
            catch (InvalidOperationException)
            {
                return this.NotFound();
            }
        }
    }
}
