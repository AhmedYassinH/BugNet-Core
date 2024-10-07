
namespace BugNetCore.Dal.Repos
{
    public class SupportRequestRepo : BaseRepo<SupportRequest>, ISupportRequestRepo
    {
        public SupportRequestRepo(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<SupportRequest> GetAllIgnoreQueryFilters(
                string? filterOn = null, string? filterQuery = null,
                string? sortBy = null, bool isAscending = true,
                int pageSize = 10, int pageNumber = 1
                )
        {
            IQueryable<SupportRequest> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, pageSize, pageNumber, sortBy, isAscending);


            // include the Bug (and then also the customer inside), and the ChatRoom (and then also the Dev inside)
            table
                .Include(s => s.Bug)
                .ThenInclude(b => b.Customer)
                .Include(s => s.ChatRoom)
                .ThenInclude(c => c.SupportDev)
                .ToList();


            return table.ToList();
        }


        public override async Task<SupportRequest> FindAsync(Guid id)
           // include the Bug (and then also the customer inside), and the ChatRoom (and then also the Dev inside)
           => await Table
                    .Include(s => s.Bug)
                    .ThenInclude(b => b.Customer)
                    .Include(s => s.ChatRoom)
                    .ThenInclude(c => c.SupportDev)
                    .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<Guid> GetCustomerIdBySupportRequestIdAsync(Guid supportRequestId)
        {
            return await Table
                .Where(x => x.Id == supportRequestId)
                .Include(x => x.Bug)
                .Select(x => x.Bug.CustomerId)
                .FirstOrDefaultAsync();
        }

        public int CountAllByUserIgnoreQueryFilters(Guid userId, string? filterOn = null, string? filterQuery = null)
        {
            IQueryable<SupportRequest> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, null, null, null);
            table.Where(r => r.Bug.CustomerId == userId || (r.ChatRoom != null && r.ChatRoom.SupportDevId == userId));
            return table.Count();
        }
    }
}
