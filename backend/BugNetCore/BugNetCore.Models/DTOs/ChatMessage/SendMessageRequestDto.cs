namespace BugNetCore.Models.DTOs.ChatMessage
{
    public class SendMessageRequestDto
    {
        public Guid SupportRequestId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; }
    }
}