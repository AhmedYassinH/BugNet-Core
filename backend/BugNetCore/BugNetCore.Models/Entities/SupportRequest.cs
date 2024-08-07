namespace BugNetCore.Models.Entities
{
    public class SupportRequest : BaseEntityWithAudit
    {
        [Required]
        public Guid BugId { get; set; } // FK

        [ForeignKey(nameof(BugId))]
        public Bug Bug { get; set; }

        [DefaultValue(SupportRequestStatus.Pending)]
        public SupportRequestStatus Status { get; set; }


        [InverseProperty(nameof(ChatRoom.SupportRequest))]
        public ChatRoom? ChatRoom { get; set; }

    }

    public enum SupportRequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Canceled,
        Closed,

    }
}
