namespace BugNetCore.Models.DTOs.ChatRoom
{
    public class LeaveRoomRequestDto
    {
        [Required]
        public Guid SupportRequestId { get; set; }
    }
}