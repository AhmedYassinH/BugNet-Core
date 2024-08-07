

namespace BugNetCore.Models.Entities.Configuration
{
    public class BaseEntityWithAuditConfiguration : IEntityTypeConfiguration<BaseEntityWithAudit>
    {
        public void Configure(EntityTypeBuilder<BaseEntityWithAudit> builder)
        {
            builder
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("now() at time zone 'utc'");

            builder
                .Property(u => u.LastModified)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .ValueGeneratedOnAddOrUpdate();

        }
    }
}
