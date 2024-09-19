using BugNetCore.Api.Hubs;

namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    public class CommentController : BaseCrudController<Comment, CommentController, CreateCommentRequestDto, UpdateCommentRequestDto, ReadCommentResponseDto>
    {
        private readonly ICommentDataService _commentDataService;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public CommentController(
            IHubContext<NotificationHub> notificationHubContext,
            ICommentDataService commentDataService,
            IAppLogging<CommentController> logger,
            ICommentRepo mainRepo,
            IMapper mapper) : base(logger, mainRepo, mapper)
        {
            _commentDataService = commentDataService;
            _notificationHubContext = notificationHubContext;
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
        public async override Task<ActionResult<ReadCommentResponseDto>> AddOneAsync(CreateCommentRequestDto entity)
        {


            if (!ModelState.IsValid)
            {
                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                     x => x.Key,
                     x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }


            Comment domainEntity = _mapper.Map<Comment>(entity);
            try
            {

                await _commentDataService.AddAndNotifyAsync(domainEntity, notificationCallBack: async (userId, notification) =>
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

            return CreatedAtAction(nameof(GetOneAsync), new { id = _mapper.Map<ReadCommentResponseDto>(domainEntity).Id }, _mapper.Map<ReadCommentResponseDto>(domainEntity));
        }

    }
}
