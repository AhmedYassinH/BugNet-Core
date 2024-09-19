namespace BugNetCore.Dal.Repos
{
    public class BugRepo : BaseRepo<Bug>, IBugRepo
    {
        public BugRepo(ApplicationDbContext context) : base(context)
        {

        }

        public virtual IEnumerable<Bug> GetAllIgnoreQueryFilters(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true,
        int pageSize = 10, int pageNumber = 1
        )
        {
            IQueryable<Bug> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, pageSize, pageNumber, sortBy, isAscending);


            // include the project, customer and dev records
            table
                .Include(b => b.Customer)
                .Include(b => b.Project)
                .Include(b => b.Dev)
                .ToList();


            return table.ToList();
        }

        public override async Task<Bug> FindAsync(Guid id)
           => await Table
                    .Include(b => b.Customer)
                    .Include(b => b.Project)
                    .Include(b => b.Dev)
                    .FirstOrDefaultAsync(x => x.Id == id);

    }
}

