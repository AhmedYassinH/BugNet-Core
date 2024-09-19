namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class UpdateSupportResponseDto : BaseEntityWithAudit
    {
        public Guid BugId { get; set; }

        public SupportRequestStatus Status { get; set; }

    }
}
