using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
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
        private readonly IHttpContextService httpContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListController"/> class.
        /// </summary>
        /// <param name="toDoListService">Service for <see cref="ToDoList"/>.</param>
        /// <param name="httpContextService">Service for http context.</param>
        public ToDoListController(IToDoListService toDoListService, IHttpContextService httpContextService)
        {
            this.toDoListService = toDoListService;
            this.httpContextService = httpContextService;
        }

        /// <summary>
        /// Handles request for getting all not archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoList"/>.</returns>
        [HttpGet]
        [Route("unarchived")]
        public ActionResult GetNotArchivedLists()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var notArchivedLists = this.toDoListService.GetNotArchivedLists();
            return this.Ok(notArchivedLists.Where(l => l.UserID == userID));
        }

        /// <summary>
        /// Handles request for getting all lists.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoList"/>.</returns>
        [HttpGet]
        public ActionResult GetLists()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var notArchivedLists = this.toDoListService.GetAllLists();
            return this.Ok(notArchivedLists.Where(l => l.UserID == userID));
        }

        /// <summary>
        /// Handles request for getting archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of archived <see cref="ToDoList"/>.</returns>
        [HttpGet]
        [Route("archived")]
        public ActionResult GetArchivedLists()
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var archivedLists = this.toDoListService.GetArchivedLists();
            return this.Ok(archivedLists.Where(l => l.UserID == userID));
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
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            var list = await this.toDoListService.GetListByID(id);
            if (list == null)
            {
                return this.NotFound();
            }

            if (list.UserID != userID)
            {
                return this.Forbid("This list does not belong to you");
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
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            if ((await this.toDoListService.GetListByID(id)).UserID != userID)
            {
                return this.Forbid("This list does not belong to you");
            }

            var newList = await this.toDoListService.CopyList(id);
            return this.Ok(newList);
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoList"/> by specified <see cref="ToDoList.Id"/>.
        /// </summary>
        /// <param name="list">Updated <see cref="ToDoList"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoList"/> is empty or <see cref="ToDoList.Id"/> in query and <see cref="ToDoList.Id"/> in object (or <see cref="ToDoList.Id"/> in found item and <see cref="ToDoList"/> in sent <see cref="ToDoList"/>) are different, NotFound response if there is no such <see cref="ToDoList"/>, Ok response with the updated <see cref="ToDoList"/>.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateList([FromBody] ToDoListUpdate list)
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            if (list.UserID != userID)
            {
                return this.Forbid("This list does not belong to you");
            }

            var updatedList = await this.toDoListService.UpdateList(list);
            return this.Ok(updatedList);
        }

        /// <summary>
        /// Handles request for InsertAsyncing <see cref="ToDoList"/>.
        /// </summary>
        /// <param name="list"><see cref="ToDoList"/> to be InsertAsynced.</param>
        /// <returns>BadRequest response incase passed <see cref="ToDoList"/> is empty or Created response with created <see cref="ToDoList"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] ToDoListCreate list)
        {
            int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
            if (list.UserID != userID)
            {
                return this.Forbid("You cannot assign your list to the other user");
            }

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
        public async Task<IActionResult> DeleteList(int id)
        {
            try
            {
                int userID = this.httpContextService.GetIdByContextUser(this.HttpContext.User);
                var list = await this.toDoListService.GetListByID(id);
                if (list != null && list.UserID != userID)
                {
                    return this.Forbid("This list does not belong to you");
                }

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
