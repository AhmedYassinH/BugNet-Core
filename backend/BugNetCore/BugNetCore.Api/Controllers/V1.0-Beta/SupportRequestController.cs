using BugNetCore.Api.Hubs;
using BugNetCore.Services.DataServices.Interfaces;

namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    public class SupportRequestController : BaseCrudController<SupportRequest, SupportRequestController, CreateSupportRequestDto, UpdateSupportRequestDto, ReadSupportResponseDto>
    {
        private readonly ISupportRequestDataService _supportRequestDataService;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public SupportRequestController(
            IHubContext<NotificationHub> notificationHubContext,
            IAppLogging<SupportRequestController> logger,
            ISupportRequestRepo mainRepo, 
            ISupportRequestDataService supportRequestDataService, 
            IMapper mapper) : base(logger, mainRepo, mapper)
        {
            _supportRequestDataService = supportRequestDataService;
            _notificationHubContext = notificationHubContext;

        }

        /// <summary>
        /// Adds a single record
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>Added record</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(201, "The execution was successful")]
        [SwaggerResponse(400, "The request was invalid")]
        [SwaggerResponse(401, "Unauthorized access attempted")]
        [SwaggerResponse(403, "Forbidden access attempted")]
        [SwaggerResponse(500, "An internal server error has occurred")]
        [HttpPost]
        public async override Task<ActionResult<ReadSupportResponseDto>> AddOneAsync(CreateSupportRequestDto entity)
        {


            if (!ModelState.IsValid)
            {
                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                     x => x.Key,
                     x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }


            SupportRequest domainEntity = _mapper.Map<SupportRequest>(entity);
            try
            {

                await _supportRequestDataService.AddAndNotifyAsync(domainEntity, notificationCallBack: async (userId, notification) =>
                {
                    await _notificationHubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
                });
            }

            catch (UnknownDatabaseException ex)
            {
                throw new customWebExceptions.WebException(ex.Message)
                {
                    Code = "DatabaseError"
                };
            }

            return CreatedAtAction(nameof(GetOneAsync), new { id = _mapper.Map<ReadSupportResponseDto>(domainEntity).Id }, _mapper.Map<ReadSupportResponseDto>(domainEntity));
        }




        /// <summary>
        /// Perform an action on a Live Support Request. (Aprrove, Reject, Cancel, Close)
        /// </summary>
        /// <param name="actOnSupportRequestDto">The request containing the action to perform, the Support Request ID to act on and a Developer Id to be assigned to the Chat Room if the request is approved.</param>
        /// <returns>
        /// Either:
        /// - 200 (OK): The Support Request action were successfully performed, and the Support Request record has been updated.
        /// - 400 (Bad Request): The request was invalid. Check the response body for details on the encountered errors.
        /// </returns>
        /// <remarks>
        /// This endpoint allows performing various actions on a Live Support Request record:
        /// - Before approval/rejection by an admin:
        ///   - Customers can "Cancel" their Live Support Requests.
        ///   - Admins can "Approve" or "Reject" pending Live Support Requests initiated by the customers.
        /// - After approval/rejection:
        ///   - Admins can still "Reject" the request at this stage.
        ///   - Admins can "Close" the Live Support Request after the chat has ended.
        /// </remarks>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The action was successful")]
        [SwaggerResponse(400, "The request was invalid")]
        [SwaggerResponse(401, "Unauthorized access attempted")]
        [SwaggerResponse(403, "Forbidden access attempted")]
        [SwaggerResponse(404, "The requested resource was not found")]
        [SwaggerResponse(500, "An internal server error has occurred")]
        //[ApiVersion("0.1-Beta")]
        // [Authorize]
        [HttpPut("act-on-support-request")]
        public async Task<ActionResult<UpdateSupportResponseDto>> ActOnBorrowingStatusAsync(ActOnSupportRequestDto actOnSupportRequestDto)
        {
            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }

            // Try updating the status of the borrowings
            UpdateSupportResponseDto updateSupportResponseDto;
            try
            {
                updateSupportResponseDto = await _supportRequestDataService.HandleSupportRequestActionAndNotifyAsync(actOnSupportRequestDto, notificationCallback: async (userId, notification) =>
                {
                    await _notificationHubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
                });

            }

            // catch (BorrowingUserNotFoundException ex)
            // {
            //     throw new customWebExceptions.NotFoundException(ex.Message)
            //     {
            //         Code = "BorrowingUserNotFound"
            //     };
            // }
            // catch (BorrowingActionForbiddenException ex)
            // {
            //     throw new customWebExceptions.ForbiddenException(ex.Message)
            //     {
            //         Code = "BorrowingActionForbidden"
            //     };
            // }
            catch (UnknownDatabaseException ex)
            {
                throw new customWebExceptions.WebException(ex.Message)
                {
                    Code = "DatabaseError"
                };
            }

            

            return updateSupportResponseDto;
        }
    }
}
