namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface ISupportRequestRepo : IBaseRepo<SupportRequest>
    {
        Task<Guid> GetCustomerIdBySupportRequestIdAsync(Guid supportRequestId);
        
    }
}
