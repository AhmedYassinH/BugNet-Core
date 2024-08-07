
namespace BugNetCore.Models.Entities.Configuration
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            new BaseEntityWithAuditConfiguration<UserNotification>().Configure(builder);
        }
    }
}
