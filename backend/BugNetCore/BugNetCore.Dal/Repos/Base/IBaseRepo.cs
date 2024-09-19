﻿namespace BugNetCore.Dal.Repos.Base
{
    public interface IBaseRepo<T> : IBaseViewRepo<T> where T : BaseEntity, new()
    {
        Task<T> FindAsync(Guid id);
        Task<T> FindAsNoTrackingAsync(Guid id);
        Task<T> FindIgnoreQueryFiltersAsync(Guid id);
        void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects);
        Task<int> AddAsync(T entity, bool persist = true);
        Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true);
        Task<int> UpdateAsync(T entity, bool persist = true);
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true);
        Task<int> DeleteAsync(Guid id, uint rowVersion, bool persist = true);
        Task<int> DeleteAsync(T entity, bool persist = true);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true);
        Task<int> SaveChangesAsync();
    }
}
