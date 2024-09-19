namespace BugNetCore.Models.DTOs.ChatRoom
{
    public class JoinRoomRequestDto
    {
        [Required]
        public Guid SupportRequestId { get; set; }

        public Guid UserId { get; set; }

    }
}