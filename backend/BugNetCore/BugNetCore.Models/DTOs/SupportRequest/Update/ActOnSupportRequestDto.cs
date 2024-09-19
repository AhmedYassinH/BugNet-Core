namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class ActOnSupportRequestDto : BaseEntity
    {
        public SupportRequestAction Action { get; set; }
        public Guid? SupportDevId { get; set; }

    }

    public enum SupportRequestAction
    {
        Approve,
        Reject,
        Cancel,
        Close,

    }
}
