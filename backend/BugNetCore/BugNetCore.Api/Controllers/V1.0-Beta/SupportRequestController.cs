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
        /// Gets all records with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="filterOn">The property to filter records on. Supports filtering by various formats:.  
        /// - Nested property filtering: Use syntax => {Property.NestedProperty}.  
        /// - Boolean property filtering: Exact value: true.  
        /// - String property filtering: Contains: "value".  
        /// - Date Time property filtering:.  
        ///     * Exact date: =2022-01-01.  
        ///     * Date greater than or equal to: &gt;=2022-01-01.  
        ///     * Date less than or equal to: &lt;=2022-01-01.  
        ///     * Dates between two dates: 2022-01-01~2022-01-02.  
        /// - Numeric property filtering:.  
        ///     * Exact value: =42.  
        ///     * Greater than or equal to: &gt;=100.  
        ///     * Less than or equal to: &lt;=50.  
        ///     * Range between two values: 10~20.  
        /// </param>
        /// <param name="filterQuery">The query string for filtering based on the specified property.</param>
        /// <param name="sortBy">The property to sort records by.</param>
        /// <param name="isAscending">Specifies the sort order (ascending or descending).</param>
        /// <param name="pageSize">The number of records to return per page (default is 10).</param>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <remarks>
        /// Supports various filtering formats, including nested properties, boolean values, string matching, date comparisons, and numeric comparisons.
        /// </remarks>
        /// <returns>All records matching the specified criteria.</returns>
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
        public async override Task<ActionResult<ReadAllRecordsResponseBaseDto<ReadSupportResponseDto>>> GetAll(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery, // Filtering
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,    // Sorting
            [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1 // Pagination
            )
        {
            _logger.LogAppCritical("Getting all records");
            IEnumerable<ReadSupportResponseDto> entityResponseDto;
            int totalNumberOfEntities;
            try
            {
                entityResponseDto = await _supportRequestDataService.GetAllWithUsersAsync(
                    filterOn, filterQuery,
                    sortBy, isAscending ?? true,
                    pageSize, pageNumber
                    );

                totalNumberOfEntities = _mainRepo.CountAllIgnoreQueryFilters(filterOn, filterQuery);
            }
            catch (FormatException ex)
            {
                throw new customWebExceptions.ValidationException(ex.Message)
                {
                    Code = "BadFilterQueryFormat"
                };
            }
            catch (ArgumentException ex)
            {
                throw new customWebExceptions.ValidationException(ex.Message)
                {
                    Code = "BadFilterOnArgument"
                };
            }


            if (entityResponseDto == null)
            {
                throw new customWebExceptions.NotFoundException("The requested resource was not found");
            }


            var readAllEntitiesResponse = new ReadAllRecordsResponseBaseDto<ReadSupportResponseDto>
            {
                TotalRecords = totalNumberOfEntities,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Records = entityResponseDto.ToList(),
            };

            return Ok(readAllEntitiesResponse);
        }


        /// <summary>
        /// Gets a single record
        /// </summary>
        /// <param name="id">Primary key of the record</param>
        /// <returns>Single record</returns>
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
        [HttpGet("{id}")]
        public async override Task<ActionResult<ReadSupportResponseDto>> GetOneAsync(Guid id)
        {
            var responseDto = await _supportRequestDataService.FindWithUsersAsync(id);

            if (responseDto == null)
            {
                throw new customWebExceptions.NotFoundException("The requested resource was not found");
            }
            return Ok(responseDto);
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
