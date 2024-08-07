
namespace BugNetCore.Models.Entities
{
    public class ChatRoom : BaseEntityWithAudit
    {
        [Required]
        public Guid SupportRequestId { get; set; } // Fk

        [ForeignKey(nameof(SupportRequestId))]
        public SupportRequest SupportRequest { get; set; }


        public Guid SupportDevId { get; set; } // Fk

        [ForeignKey(nameof(SupportDevId))]
        public User SupportDev { get; set; }


        [InverseProperty(nameof(ChatMessage.ChatRoom))]
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    }
}
