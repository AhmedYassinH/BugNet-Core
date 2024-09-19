namespace BugNetCore.Services.DataServices.Interfaces
{
    public delegate Task NotificationCallback(Guid userId, ReadNotificationMinimalResponseDto notification);
    public interface IBaseDataService<TEntity> where TEntity : BaseEntityWithAudit, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending,
            int pageSize, int pageNumber);
        Task<TEntity> FindAsync(Guid id);
        Task<TEntity> UpdateAsync(TEntity entity, bool persist = true);
        Task DeleteAsync(TEntity entity, bool persist = true);
        Task<TEntity> AddAsync(TEntity entity, bool persist = true);

        void ResetChangeTracker() { }
        
    }
}
