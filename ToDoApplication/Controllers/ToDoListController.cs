using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.DTOs;
using TODOListDomainModel.Classes;
using TODOListDomainModel.Interfaces;

namespace ToDoApplication.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="ToDoList"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("lists")]
    public class ToDoListController : ControllerBase
    {
        private readonly IRepository<ToDoList> listRepository;
        private readonly IRepository<ToDoItem> itemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListController"/> class.
        /// </summary>
        /// <param name="listRepository">Repository for <see cref="ToDoList"/>.</param>
        /// <param name="itemRepository">Repository for <see cref="ToDoItem"/>.</param>
        public ToDoListController(IRepository<ToDoList> listRepository, IRepository<ToDoItem> itemRepository)
        {
            this.listRepository = listRepository;
            this.itemRepository = itemRepository;
        }

        /// <summary>
        /// Handles request for getting all not archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of all <see cref="ToDoList"/>.</returns>
        [HttpGet]
        public async Task<ActionResult> GetNotArchivedLists()
        {
            var lists = await this.listRepository.GetAll();
            var returnLists = lists.Where(l => !l.IsArchived).Select(l => new ToDoListWithCompleteness
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                NotStarted = l.Items.Count(i => i.Status == Status.NotStarted),
                InProcess = l.Items.Count(i => i.Status == Status.InProcess),
                Completed = l.Items.Count(i => i.Status == Status.Completed),
            });
            return this.Ok(returnLists);
        }

        /// <summary>
        /// Handles request for getting archived <see cref="ToDoList"/>.
        /// </summary>
        /// <returns>Http response with the list of archived <see cref="ToDoList"/>.</returns>
        [HttpGet]
        [Route("archived")]
        public async Task<ActionResult> GetArchivedLists()
        {
            var lists = await this.listRepository.GetAll();
            var returnLists = lists.Where(l => l.IsArchived).Select(l => new ToDoListWithCompleteness
            {
                Id = l.Id,
                Title = l.Title,
                IsArchived = l.IsArchived,
                NotStarted = l.Items.Count(i => i.Status == Status.NotStarted),
                InProcess = l.Items.Count(i => i.Status == Status.InProcess),
                Completed = l.Items.Count(i => i.Status == Status.Completed),
            });
            return this.Ok(returnLists);
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
            var list = await this.listRepository.GetByID(id);
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
            var list = await this.listRepository.GetByID(id);
            if (list == null)
            {
                return this.NotFound();
            }

            var newList = await this.listRepository.Insert(new ToDoList { Title = list.Title + " (Copy)", IsArchived = list.IsArchived });
            newList.Items = new List<ToDoItem>();

            var items = list.Items.ToList();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    await this.itemRepository.Insert(new ToDoItem { Title = item.Title, Description = item.Description, Deadline = item.Deadline, Status = item.Status, CreatedAt = item.CreatedAt, Remind = item.Remind, ToDoListID = newList.Id, TodoList = newList, Starred = item.Starred, IsHidden = item.IsHidden });
                }
            }

            return this.Ok(new ToDoListWithCompleteness
            {
                Id = newList.Id,
                Title = newList.Title,
                IsArchived = newList.IsArchived,
                NotStarted = newList.Items.Count(i => i.Status == Status.NotStarted),
                InProcess = newList.Items.Count(i => i.Status == Status.InProcess),
                Completed = newList.Items.Count(i => i.Status == Status.Completed),
            });
        }

        /// <summary>
        /// Handles request for updating <see cref="ToDoList"/> by specified <see cref="ToDoList.Id"/>.
        /// </summary>
        /// <param name="id"><see cref="ToDoList.Id"/> to be searched by.</param>
        /// <param name="list">Updated <see cref="ToDoList"/>.</param>
        /// <returns>BadRequest response if <see cref="ToDoList"/> is empty or <see cref="ToDoList.Id"/> in query and <see cref="ToDoList.Id"/> in object (or <see cref="ToDoList.Id"/> in found item and <see cref="ToDoList"/> in sent <see cref="ToDoList"/>) are different, NotFound response if there is no such <see cref="ToDoList"/>, Ok response with the updated <see cref="ToDoList"/>.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditList(int id, [FromBody] ToDoList list)
        {
            if (this.ModelState.IsValid)
            {
                if (list.Id != id)
                {
                    return this.BadRequest("Id in query and Id of object are different.");
                }

                var foundList = await this.listRepository.GetByID(id);

                if (foundList == null)
                {
                    return this.NotFound();
                }

                if (foundList.Id != list.Id)
                {
                    return this.BadRequest("ToDo list Id in real object and ToDo list Id of sent object are different.");
                }

                this.listRepository.Update(list);

                return this.Ok(list);
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
            if (list == null)
            {
                return this.BadRequest();
            }

            await this.listRepository.Insert(list);

            return this.Created("todolists", list);
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
            var foundList = await this.listRepository.GetByID(id);

            if (foundList == null)
            {
                return this.NotFound();
            }

            this.listRepository.Delete(foundList);

            return this.Accepted();
        }
    }
}
