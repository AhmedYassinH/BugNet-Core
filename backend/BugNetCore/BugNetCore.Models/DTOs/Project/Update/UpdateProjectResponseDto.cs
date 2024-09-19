namespace BugNetCore.Models.DTOs.Project
{
    public class UpdateProjectResponseDto : BaseEntityWithAudit
    {

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ProjectStatus? Status { get; set; }

    }
}
