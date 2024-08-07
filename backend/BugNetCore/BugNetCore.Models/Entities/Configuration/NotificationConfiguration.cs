namespace BugNetCore.Models.Entities.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            new BaseEntityWithAuditConfiguration<Notification>().Configure(builder);
        }
    }
}
