﻿namespace BugNetCore.Dal.Repos.Base
{
    public abstract class BaseRepo<T> : BaseViewRepo<T>, IBaseRepo<T> where T : BaseEntity, new()
    {
        protected BaseRepo(ApplicationDbContext context) : base(context) { }


        public virtual async Task<T> FindAsync(Guid id)
            => await Table.FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task<T> FindAsNoTrackingAsync(Guid id)
            => await Table.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task<T> FindIgnoreQueryFiltersAsync(Guid id)
            => await Table.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);

        public virtual void ExecuteParameterizedQuery(string sql, object[] sqlParametersObjects)
            => Context.Database.ExecuteSqlRaw(sql, sqlParametersObjects);

        public virtual async Task<int> AddAsync(T entity, bool persist = true)
        {
            await Table.AddAsync(entity);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            await Table.AddRangeAsync(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual async Task<int> UpdateAsync(T entity, bool persist = true)
        {
            Table.Update(entity);
            //Context.ChangeTracker.DetectChanges();
            //Console.WriteLine(Context.ChangeTracker.DebugView.LongView);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        public async Task<int> DeleteAsync(Guid id, uint rowVersion, bool persist = true)
        {
            var entity = new T { Id = id, RowVersion = rowVersion };
            Context.Entry(entity).OriginalValues["RowVersion"] = rowVersion;
            Context.Entry(entity).State = EntityState.Deleted;
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual async Task<int> DeleteAsync(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? await SaveChangesAsync() : 0;
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? await SaveChangesAsync() : 0;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {

                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                throw new DbUpdateConcurrencyException("Record has been modified or deleted by another user", ex);
            }
            catch (Exception ex)
            {
                throw new UnknownDatabaseException("Fallback Database Error, report unknown error", ex);
            }

        }


    }
}
