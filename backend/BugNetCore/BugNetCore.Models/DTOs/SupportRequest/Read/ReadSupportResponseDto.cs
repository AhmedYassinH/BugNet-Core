namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class ReadSupportResponseDto : BaseEntityWithAudit
    {
        public Guid BugId { get; set; }

        public SupportRequestStatus Status { get; set; }

        public ReadBugResponseDto? Bug { get; set; }

        public ReadUserResponseDto? Customer { get; set; }

        public ReadUserResponseDto? SupportDev { get; set; }

    }
}
