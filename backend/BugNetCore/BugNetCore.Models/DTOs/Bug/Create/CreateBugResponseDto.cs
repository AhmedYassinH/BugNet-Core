namespace BugNetCore.Models.DTOs.Bug
{
    public class CreateBugResponseDto : BaseEntityWithAudit
    {
        public string Description { get; set; }

        public BugCategory Category { get; set; }

        public BugSeverity CustomerAssignedSeverity { get; set; }

        public BugStatus Status { get; set; }

        public string? Screenshot { get; set; }

        public ReadProjectResponseDto Project { get; set; }

        public ReadUserResponseDto Customer { get; set; }

    }
}
