namespace BugNetCore.Models.DTOs.Project
{
    public class UpdateProjectRequestDto : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ProjectStatus? Status { get; set; }

    }
}
