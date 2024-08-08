namespace BugNetCore.Models.Entities.Configuration.Base
{
    public class BaseEntityWithAuditConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntityWithAudit
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("now() at time zone 'utc'");

            builder
                .Property(u => u.LastModified)
                .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAddOrUpdate();

            builder
            .Property(u => u.RowVersion)
            .IsConcurrencyToken()
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsRowVersion();
        }
    }
}
