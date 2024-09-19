namespace BugNetCore.Models.Entities
{
    public class Notification : BaseEntityWithAudit
    {
        public NotificationType Type { get; set; }

        [Required]
        public string Message { get; set; }

        public Guid? BugId { get; set; } // FK

        [ForeignKey(nameof(BugId))]
        public Bug Bug { get; set; }

        public Guid? SupportRequestId { get; set; } // FK

        [ForeignKey(nameof(SupportRequestId))]
        public SupportRequest SupportRequest { get; set; }

        [InverseProperty(nameof(UserNotification.Notification))]
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();


    }

    public enum NotificationType
    {
        BugCreation,
        Comment,
        ChatInvitation,
        SupportRequest,
        BugAssignment,

    }
}
