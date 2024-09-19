namespace BugNetCore.Models.DTOs.Project
{
    public class ReadProjectResponseDto : BaseEntityWithAudit
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public ProjectStatus? Status { get; set;}
       
    }
}
