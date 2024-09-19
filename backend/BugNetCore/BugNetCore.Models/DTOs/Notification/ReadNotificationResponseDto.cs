namespace BugNetCore.Models.DTOs.Notification
{
    public class ReadNotificationResponseDto : BaseEntityWithAudit
    {
        public NotificationType Type { get; set; }

        public string Message { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

    }
}
