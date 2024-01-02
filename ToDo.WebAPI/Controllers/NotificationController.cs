using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.DomainModel.Models;
using ToDo.Services.DTOs;
using ToDo.Services.Interfaces;

namespace ToDo.WebAPI.Controllers
{
    /// <summary>
    /// Controller handling requests for <see cref="Notification"/>.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        private readonly IHttpContextService httpContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">Notification service.</param>
        /// <param name="httpContextService">Http context service.</param>
        public NotificationController(INotificationService notificationService, IHttpContextService httpContextService)
        {
            this.notificationService = notificationService;
            this.httpContextService = httpContextService;
        }

        /// <summary>
        /// Returns Action result with all notifications for given user.
        /// </summary>
        /// <param name="userId">Id of a user to search by.</param>
        /// <returns>HTTP response: OK with list of notifications (may be empty), FORBID if user does not have access to it.</returns>
        [HttpGet]
        [Route("users/:id")]
        public IActionResult GetAllByUserId(int userId)
        {
            if (userId != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.Forbid("You do not have access to this user's notifications.");
            }

            return this.Ok(this.notificationService.GetByUserId(userId));
        }

        /// <summary>
        /// Deletes notification by id.
        /// </summary>
        /// <param name="id">Notification id to delete.</param>
        /// <returns>HTTP response: ACCEPTED if successfully deleted, FORBID if user does not have access to it, NOTFOUND if notification to delete not found.</returns>
        [HttpDelete]
        [Route(":id")]
        public async Task<IActionResult> Delete(int id)
        {
            var notificationToDelete = await this.notificationService.GetNotificationAsync(id);

            if (notificationToDelete == null)
            {
                return this.NotFound("Notification not found.");
            }

            if (notificationToDelete.RecipientId != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.Forbid("You do not have access to this user's notifications.");
            }

            this.notificationService.Delete(id);

            return this.Accepted();
        }

        /// <summary>
        /// Updated notification state.
        /// </summary>
        /// <param name="dto">DTO with notification id and notification state to update.</param>
        /// <returns>HTTP response: OK with updaetd notification, FORBID if user does not have access to it, NOTFOUND if notification to delete not found.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateState([FromBody] NotificationStateUpdate dto)
        {
            var notificationToUpdate = await this.notificationService.GetNotificationAsync(dto.Id);
            if (notificationToUpdate == null)
            {
                return this.NotFound("Notification not found");
            }

            if (notificationToUpdate.RecipientId != this.httpContextService.GetIdByContextUser(this.HttpContext.User))
            {
                return this.Forbid("You do not have access to this user's notifications.");
            }

            var updatedNotification = await this.notificationService.UpdateNotificationStateAsync(dto);

            return this.Ok(updatedNotification);
        }
    }
}
