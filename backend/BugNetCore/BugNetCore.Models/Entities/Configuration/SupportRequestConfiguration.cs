
namespace BugNetCore.Models.Entities.Configuration
{
    public class SupportRequestConfiguration : IEntityTypeConfiguration<SupportRequest>
    {
        public void Configure(EntityTypeBuilder<SupportRequest> builder)
        {
            new BaseEntityWithAuditConfiguration<SupportRequest>().Configure(builder);
        }
    }
}
