namespace BugNetCore.Models.Entities
{
    public class Notification : BaseEntityWithAudit
    {
        public NotificationType Type { get; set; }

        [Required]
        public string Message { get; set; }

        [InverseProperty(nameof(UserNotification.Notification))]
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

    }

    public enum NotificationType
    {
        Bug,
        Comment,
        ChatInvitation,
        BugRequest,
        BugAssignment,

    }
}
