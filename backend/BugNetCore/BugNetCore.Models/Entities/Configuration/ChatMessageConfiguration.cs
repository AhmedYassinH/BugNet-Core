
namespace BugNetCore.Models.Entities.Configuration
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            new BaseEntityWithAuditConfiguration<ChatMessage>().Configure(builder);
        }
    }
}
