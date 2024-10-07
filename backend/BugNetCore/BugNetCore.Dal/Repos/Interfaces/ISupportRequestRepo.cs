namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface ISupportRequestRepo : IBaseRepo<SupportRequest>
    {
        Task<Guid> GetCustomerIdBySupportRequestIdAsync(Guid supportRequestId);

        int CountAllByUserIgnoreQueryFilters(Guid userId, string? filterOn = null, string? filterQuery = null);

    }
}
