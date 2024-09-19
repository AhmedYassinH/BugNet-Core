namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface IUserRepo : IBaseRepo<User>
    {
        Task<User> FindByEmailAsNoTrackingAsync(string email);
        Task<IEnumerable<Guid>> GetAdminIdsAsync();

    }
}
