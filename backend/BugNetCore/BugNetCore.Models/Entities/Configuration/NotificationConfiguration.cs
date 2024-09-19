
namespace BugNetCore.Models.Entities.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            new BaseEntityWithAuditConfiguration<Notification>().Configure(builder);

            builder
                .HasOne(n => n.Bug)
                .WithMany()
                .HasForeignKey(n => n.BugId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(n => n.SupportRequest)
                .WithMany()
                .HasForeignKey(n => n.SupportRequestId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
