
namespace BugNetCore.Models.Entities.Configuration
{
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            new BaseEntityWithAuditConfiguration<UserProject>().Configure(builder);
        }
    }
}
