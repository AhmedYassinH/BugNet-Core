namespace BugNetCore.Models.Entities
{
    public class Project : BaseEntityWithAudit
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [DefaultValue(ProjectStatus.Active)]
        public ProjectStatus? Status { get; set; }

        [InverseProperty(nameof(Bug.Project))]
        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();

        [InverseProperty(nameof(UserProject.Project))]
        public ICollection<UserProject> ProjectUsers { get; set; } = new List<UserProject>();

    }


    public enum ProjectStatus
    {
        Active,
        Deprecated,
        Terminated,

    }
}
