
using Microsoft.EntityFrameworkCore.Design;
namespace Library.Dal.EfStructures
{

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = @"Host=localhost;Port=5432;Database=testingdb;Username=postgres;Password=123";
            optionsBuilder.UseNpgsql(connectionString);
            //optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            Console.WriteLine(connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}