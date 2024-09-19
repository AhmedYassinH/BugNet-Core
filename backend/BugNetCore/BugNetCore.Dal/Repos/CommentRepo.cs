namespace BugNetCore.Dal.Repos
{
    public class CommentRepo : BaseRepo<Comment>, ICommentRepo
    {
        public CommentRepo(ApplicationDbContext context) : base(context)
        {
        }

        public virtual IEnumerable<Comment> GetAllIgnoreQueryFilters(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true,
        int pageSize = 10, int pageNumber = 1
        )
        {
            IQueryable<Comment> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, pageSize, pageNumber, sortBy, isAscending);


            // include the sender and the bug
            table
                .Include(b => b.Sender)
                .Include(b => b.Bug)
                .ToList();


            return table.ToList();
        }

        public override async Task<Comment> FindAsync(Guid id)
           => await Table
                    .Include(b => b.Sender)
                    .Include(b => b.Bug)
                    .ThenInclude(b => b!.Project)
                    .FirstOrDefaultAsync(x => x.Id == id);
    }
}
