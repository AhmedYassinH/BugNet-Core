namespace BugNetCore.Models.Entities
{
    public class UserNotification : BaseEntityWithAudit
    {
        [Required]
        public Guid UserId { get; set; } // FK

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public Guid NotificationId { get; set; } // FK

        [ForeignKey(nameof(NotificationId))]
        public Notification Notification { get; set; }

        public bool IsRead { get; set; }    
    }
}
