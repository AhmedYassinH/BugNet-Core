namespace BugNetCore.Models.Entities
{
    public class ChatMessage : BaseEntityWithAudit
    {
        [Required]
        public Guid ChatRoomId { get; set; } // FK

        [ForeignKey(nameof(ChatRoomId))]
        public ChatRoom ChatRoom { get; set; }


        [Required]
        public Guid SenderId { get; set; } // FK

        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }


        [Required]
        public string MessageText { get; set; }

    }
}
