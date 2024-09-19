namespace BugNetCore.Models.DTOs.ChatMessage
{
    public class ReceiveMessageResponseDto
    {
        public Guid SenderId { get; set; } // FK

        public string SenderName { get; set; }

        public string MessageText { get; set; }

        public DateTime SentAt { get; set; }
    }
}