namespace BugNetCore.Models.DTOs.Notification
{
    public class ReadNotificationMinimalResponseDto 
    {
        public Guid NotificationId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, Guid> AdditionalInfo { get; set; } = new Dictionary<string, Guid>();


    }
}
