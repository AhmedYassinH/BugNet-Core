namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0-Beta")]
    public class ChatMessageController : ControllerBase
    {
        private readonly IAppLogging<ChatMessageController> _logger;
        private readonly IChatMessageRepo _mainRepo;
        private readonly IMapper _mapper;

        public ChatMessageController(IAppLogging<ChatMessageController> logger, IChatMessageRepo mainRepo, IMapper mapper)
        {
            _logger = logger;
            _mainRepo = mainRepo;
            _mapper = mapper;
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
        public async virtual Task<ActionResult<IEnumerable<ReadMessageResponseDto>>> GetAll(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery, // Filtering
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,    // Sorting
            [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1 // Pagination
            )
        {
            IEnumerable<ChatMessage> entities;
            try
            {
                entities = _mainRepo.GetAllIgnoreQueryFilters(
                    filterOn, filterQuery,
                    sortBy, isAscending ?? true,
                    pageSize, pageNumber
                    );
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


            if (entities == null)
            {
                throw new customWebExceptions.NotFoundException("The requested resource was not found");
            }


            var entityResponseDto = _mapper.Map<IEnumerable<ReadMessageResponseDto>>(entities);
            return Ok(entityResponseDto);
        }
    }
}
