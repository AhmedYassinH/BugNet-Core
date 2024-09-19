namespace BugNetCore.Models.DTOs.Project
{
    public class CreateProjectResponseDto : BaseEntityWithAudit
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ProjectStatus? Status { get; set; }

    }
}
