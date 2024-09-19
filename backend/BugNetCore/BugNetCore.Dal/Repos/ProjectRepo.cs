namespace BugNetCore.Dal.Repos
{
    public class ProjectRepo : BaseRepo<Project>, IProjectRepo
    {
        public ProjectRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
