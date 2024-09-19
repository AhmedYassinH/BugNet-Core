namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class ReadSupportResponseDto : BaseEntityWithAudit
    {
        public Guid BugId { get; set; }

        public SupportRequestStatus Status { get; set; }


    }
}
