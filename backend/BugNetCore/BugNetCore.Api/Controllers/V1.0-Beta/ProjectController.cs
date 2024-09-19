namespace BugNetCore.Api.Controllers.V1._0_Beta
{
    public class ProjectController : BaseCrudController<Project, ProjectController, CreateProjectRequestDto, UpdateProjectRequestDto, ReadProjectResponseDto>
    {
        public ProjectController(IAppLogging<ProjectController> logger, IProjectRepo mainRepo, IMapper mapper) : base(logger, mainRepo, mapper)
        {
        }
    }
}
