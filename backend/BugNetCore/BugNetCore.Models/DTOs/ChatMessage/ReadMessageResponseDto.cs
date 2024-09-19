namespace BugNetCore.Models.DTOs.ChatMessage
{
    public class ReadMessageResponseDto : BaseEntityWithAudit
    {
        public Guid SenderId { get; set; } // FK

        public string MessageText { get; set; }
    }
}