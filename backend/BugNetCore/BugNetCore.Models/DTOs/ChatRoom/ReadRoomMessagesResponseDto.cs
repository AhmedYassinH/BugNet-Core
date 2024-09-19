namespace BugNetCore.Models.DTOs.ChatRoom
{
    public class ReadRoomMessagesResponseDto : BaseEntity
    {
        public List<ReadMessageResponseDto> Messages { get; set; }
    }
}