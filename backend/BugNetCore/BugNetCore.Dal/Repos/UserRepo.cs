namespace BugNetCore.Dal.Repos
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> FindByEmailAsNoTrackingAsync(string email)
        {
            return await Table
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<IEnumerable<Guid>> GetAdminIdsAsync()
        {
            return await Table
                .Where(x => x.UserRole == Role.Admin)
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}
