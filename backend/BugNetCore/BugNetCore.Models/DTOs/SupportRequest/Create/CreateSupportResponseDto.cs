namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class CreateSupportResponseDto : BaseEntityWithAudit
    {
        public ReadBugResponseDto Bug { get; set; }

        public SupportRequestStatus Status { get; set; }

    }
}
