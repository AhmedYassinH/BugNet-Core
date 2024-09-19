

namespace BugNetCore.Api.Controllers.V1_0_Beta
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0-Beta")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationDataService _notificationDataService;

        public NotificationController(INotificationDataService notificationDataService)
        {
            _notificationDataService = notificationDataService;
        }

        /// <summary>
        /// Gets all notifications for a user.
        /// </summary>
        /// <param name="userId">Primary key of the the user to get his/her related notifications</param>
        /// <returns>All notification records related to a user.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(401, "Unauthorized access attempted")]
        [SwaggerResponse(403, "Forbidden access attempted")]
        [SwaggerResponse(404, "The requested resource was not found")]
        [SwaggerResponse(500, "An internal server error has occurred")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadNotificationMinimalResponseDto>>> GetAllUserNotificationAsync(
            [FromQuery] Guid userId
            )
        {
            IEnumerable<ReadNotificationMinimalResponseDto> notifications;
            try
            {
                notifications = await _notificationDataService.GetAllUserNotificationsAsync(userId);
            }
            catch (Exception ex)
            {
                throw;
            }

            if (notifications == null)
            {
                throw new customWebExceptions.NotFoundException("The requested resource was not found");
            }


            return Ok(notifications);
        }



        /// <summary>
        /// Marks a notification as read for a user.
        /// </summary>
        /// <param name="notificationId">Primary key of the user notification.</param>
        /// <returns>No content.</returns>
        [HttpPut("read/{notificationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(204, "The notification was marked as read")]
        [SwaggerResponse(401, "Unauthorized access attempted")]
        [SwaggerResponse(403, "Forbidden access attempted")]
        [SwaggerResponse(404, "The requested resource was not found")]
        [SwaggerResponse(500, "An internal server error has occurred")]
        [Authorize]
        public async Task<IActionResult> ReadNotificationAsync(Guid notificationId)
        {
            // Extract the userId from the Claims Principle in the context
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                await _notificationDataService.ReadNotificationAsync(notificationId: notificationId, userId: userId);
            }
            catch (Exception ex)
            {
                throw;
            }

            return NoContent();
        }



    }
}