using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BugNetCore.Models.Entities.Configuration.Base
{
    public class BaseEntityWithAuditConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntityWithAudit
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {

           
            builder
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("now()");

            builder
                .Property(u => u.LastModified)
                .HasDefaultValueSql("now()")
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
