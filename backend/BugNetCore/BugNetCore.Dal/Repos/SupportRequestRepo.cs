
namespace BugNetCore.Dal.Repos
{
    public class SupportRequestRepo : BaseRepo<SupportRequest>, ISupportRequestRepo
    {
        public SupportRequestRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Guid> GetCustomerIdBySupportRequestIdAsync(Guid supportRequestId)
        {
            return await Table
                .Where(x => x.Id == supportRequestId)
                .Include(x => x.Bug)
                .Select(x => x.Bug.CustomerId)
                .FirstOrDefaultAsync();
        }
    }
}
