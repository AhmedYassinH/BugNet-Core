using BugNetCore.Api.Hubs;

namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    public class BugController : BaseCrudController<Bug, BugController, CreateBugRequestDto, UpdateBugRequestDto, ReadBugResponseDto>
    {
        private readonly IBugDataService _bugDataService;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BugController(
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            IHubContext<NotificationHub> notificationHubContext,
            IBugDataService bugDataService,
            IAppLogging<BugController> logger,
            IBugRepo mainRepo,
            IMapper mapper) : base(logger, mainRepo, mapper)
        {
            _bugDataService = bugDataService;
            _notificationHubContext = notificationHubContext;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;

        }


        /// <inheritdoc />
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
        [Authorize]
        public async override Task<ActionResult<ReadBugResponseDto>> AddOneAsync([FromForm] CreateBugRequestDto entity)
        {


            if (!ModelState.IsValid)
            {
                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                     x => x.Key,
                     x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }


            Bug domainEntity = _mapper.Map<Bug>(entity);
            try
            {
                // If Screenshot exists, upload it to the wwwroot/Images/Bugs/{Screenshot.FileName}
                if (entity.ScreenshotFile != null)
                {
                    var imageName = Path.GetFileName(entity.ScreenshotFile.FileName);

                    // Get the path of the wwwroot folder
                    var webRootPath = _webHostEnvironment.WebRootPath;

                    // Image path in the StaticFiles Folder
                    var localImagePath = Path.Combine(webRootPath, "Images", "Bugs", $"{imageName}");
                    // Image URI
                    var imageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/Bugs/{imageName}";

                    // Upload the image to the local StaticFiles Folder
                    using var stream = new FileStream(localImagePath, FileMode.Create);
                    // Write the image to the stream
                    await entity.ScreenshotFile.CopyToAsync(stream);

                    // Map the url to the dto
                    domainEntity.Screenshot = imageUrl;
                }
                // Extract the userId from the Claims Principle in the context
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                domainEntity.CustomerId = userId;
                await _bugDataService.AddAndNotifyAsync(domainEntity, notificationCallBack: async (userId, notification) =>
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

            return CreatedAtAction(nameof(GetOneAsync), new { id = _mapper.Map<ReadBugResponseDto>(domainEntity).Id }, _mapper.Map<ReadBugResponseDto>(domainEntity));
        }


        // TODO: add endpoint to assign bug to dev and notify them


    }
}
