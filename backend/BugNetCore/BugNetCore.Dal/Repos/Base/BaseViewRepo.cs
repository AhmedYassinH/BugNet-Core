using System.Linq.Dynamic.Core;

namespace BugNetCore.Dal.Repos.Base
{
    public abstract class BaseViewRepo<T> : IBaseViewRepo<T> where T : class, new()
    {
        private bool disposedValue;

        public DbSet<T> Table { get; }

        public ApplicationDbContext Context { get; }

        protected BaseViewRepo(ApplicationDbContext context)
        {
            Context = context;
            Table = Context.Set<T>();

        }


        public virtual IEnumerable<T> ExecuteSqlString(string sqlString)
        {
            return Table.FromSqlRaw(sqlString);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Table.AsQueryable();
        }


        public virtual IEnumerable<T> GetAllIgnoreQueryFilters(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageSize = 10, int pageNumber = 1
            )
        {
            IQueryable<T> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, pageSize, pageNumber, sortBy, isAscending);

            return table.ToList();
        }


        protected IQueryable<T> GetAllIgnoreQueryFiltersHelper(string? filterOn, string? filterQuery, int? pageSize, int? pageNumber, string? sortBy, bool isAscending = true)
        {
            var table = Table.IgnoreQueryFilters().AsQueryable();

            // Filtering
            if (!String.IsNullOrWhiteSpace(filterOn) && !String.IsNullOrWhiteSpace(filterQuery))
            {

                // Get the PropertyInfo object for the property to filter on using reflection
                var propertyInfo = typeof(T).GetProperty(filterOn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    // Construct the predicate dynamically
                    var predicate = LinqHelpers.BuildWherePredicate<T>(propertyInfo, filterQuery);

                    // Apply the predicate to filter the query
                    table = table.Where(predicate);
                }

                // Else check if the filter on follow the <string.strning> pattern
                else if (filterOn.Contains("."))
                {
                    // This time, build the predicate dynamically using System.Linq.Dynamic.Core Package
                    string constructedLambdaString = LinqHelpers.BuildWherePredicateForNestedProperty<T>(filterOn, filterQuery);

                    // Apply the predicate to filter the query
                    table = table.Where($"{constructedLambdaString}");

                }
                else
                {

                    throw new ArgumentException("Invalid filterOn property");
                }
            }


            // Sorting
            if (!String.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {

                    var function = LinqHelpers.BuildOrderByFunction<T>(propertyInfo);

                    table = isAscending ? table.OrderBy(function) : table.OrderByDescending(function);
                }


            }

            // Pagination
            if (pageSize != null && pageNumber != null)
            {
                int toSkip = ((int)pageNumber - 1) * (int)pageSize;
                table = table.Skip(toSkip).Take((int)pageSize);
            }

            return table;
        }
        public int CountAllIgnoreQueryFilters(string? filterOn = null, string? filterQuery = null)
        {
            IQueryable<T> table = GetAllIgnoreQueryFiltersHelper(filterOn, filterQuery, null, null, null);

            return table.Count();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }

                // No unmanaged resources

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }

}
