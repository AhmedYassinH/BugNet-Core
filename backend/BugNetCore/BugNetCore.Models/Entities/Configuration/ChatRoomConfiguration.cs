
namespace BugNetCore.Models.Entities.Configuration
{
    public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            new BaseEntityWithAuditConfiguration<ChatRoom>().Configure(builder);
        }
    }
}
