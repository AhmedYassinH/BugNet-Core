namespace BugNetCore.Dal.Initialization
{
    public static class SampleDataInitializer
    {
        internal static void DropAndCreateDatabase(ApplicationDbContext context)
        {

            context.Database.EnsureDeleted();
            context.Database.Migrate();

        }

        internal static void SeedData(ApplicationDbContext context)
        {

            try
            {
                ProcessInsert(context, context.Users, SampleData.Users);
                ProcessInsert(context, context.Projects, SampleData.Projects);
                ProcessInsert(context, context.UserProjects, SampleData.UserProjects);
                ProcessInsert(context, context.Bugs, SampleData.Bugs);
                ProcessInsert(context, context.Comments, SampleData.Comments);
                ProcessInsert(context, context.SupportRequests, SampleData.SupportRequests);
                ProcessInsert(context, context.ChatRooms, SampleData.ChatRooms);
                ProcessInsert(context, context.ChatMessages, SampleData.ChatMessages);
                ProcessInsert(context, context.Notifications, SampleData.Notifications);
                ProcessInsert(context, context.UserNotifications, SampleData.UserNotifications);

            }
            catch (Exception ex)
            {
                throw;

            }


            static void ProcessInsert<TEntity>(
                ApplicationDbContext context, DbSet<TEntity> table, List<TEntity> records) where TEntity : BaseEntity
            {
                // Do nothing if table already has data
                if (table.Any())
                {
                    return;
                }

                // Use the strategy to wrap all calls into single session, to enable identity insert for all queries
                IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
                strategy.Execute(() =>
                {
                    using var transaction = context.Database.BeginTransaction();

                    try
                    {

                        table.AddRange(records);
                        context.SaveChanges();

                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                    }


                });

            }
        }



        public static void InitializeData(ApplicationDbContext context)
        {
            DropAndCreateDatabase(context);
            SeedData(context);
        }
    }
}
