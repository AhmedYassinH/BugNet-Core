
namespace BugNetCore.Models.Entities.Configuration
{
    public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            new BaseEntityWithAuditConfiguration<Bug>().Configure(builder);
        }
    }
}
