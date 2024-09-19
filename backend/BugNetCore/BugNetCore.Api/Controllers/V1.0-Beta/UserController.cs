namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    public class UserController : BaseCrudController<User, UserController, CreateUserRequestDto, UpdateUserRequestDto, ReadUserResponseDto>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppLogging<UserController> logger,
            IUserRepo mainRepo,
            IMapper mapper) : base(logger, mainRepo, mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;

        }


        /// <summary>
        /// Updates a single record
        /// </summary>
        /// <param name="id">Primary key of the record to update</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Updated record</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(400, "The request was invalid")]
        [SwaggerResponse(401, "Unauthorized access attempted")]
        [SwaggerResponse(403, "Forbidden access attempted")]
        [SwaggerResponse(404, "The requested resource was not found")]
        [SwaggerResponse(500, "An internal server error has occurred")]
        [HttpPut("{id}")]
        public async override Task<ActionResult<ReadUserResponseDto>> UpdateOneAsync([FromRoute] Guid id, [FromForm] UpdateUserRequestDto entity)
        {

            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }


            if (id != entity.Id)
            {
                _logger.LogAppWarning("Id in the route and the entity do not match");
                throw new customWebExceptions.ConflictException
                    ("Id in the route and the entity do not match");
            }




            User domainEntity;

            try
            {
                domainEntity = await _mainRepo.FindAsync(id);
                if (domainEntity == null)
                {
                    _logger.LogAppWarning("Entity not found");
                    throw new customWebExceptions.NotFoundException("Entity not found");
                }
                _mapper.Map(entity, domainEntity);

                // If a picture exists, upload it to the wwwroot/Images/Users/{picture.FileName}
                if (entity.PictureFile != null)
                {
                    var imageName = Path.GetFileName(entity.PictureFile.FileName);

                    // Get the path of the wwwroot folder
                    var webRootPath = _webHostEnvironment.WebRootPath;

                    // Image path in the StaticFiles Folder
                    var localImagePath = Path.Combine(webRootPath, "Images", "Users", $"{imageName}");
                    // Image URI
                    var imageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/Users/{imageName}";

                    // Upload the image to the local StaticFiles Folder
                    using var stream = new FileStream(localImagePath, FileMode.Create);
                    // Write the image to the stream
                    await entity.PictureFile.CopyToAsync(stream);

                    // Map the url to the dto
                    domainEntity.Picture = imageUrl;
                }
                await _mainRepo.UpdateAsync(domainEntity);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new customWebExceptions.WebException(ex.Message)
                {
                    Code = "ConcurrencyError"
                };
            }
            catch (UnknownDatabaseException ex)
            {
                throw new customWebExceptions.WebException(ex.Message)
                {
                    Code = "DatabaseError"
                };
            }




            return Ok(_mapper.Map<ReadUserResponseDto>(domainEntity));
        }
    }
}
