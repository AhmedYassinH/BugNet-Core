namespace BugNetCore.Models.Entities
{
    public class UserProject : BaseEntityWithAudit
    {
        [Required]
        public Guid UserId { get; set; } // FK

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }


        [Required]
        public Guid ProjectId { get; set; } // FK

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }


        [DefaultValue(UserProjectRole.Member)]
        public UserProjectRole Role { get; set; }

    }

    public enum UserProjectRole
    {
        Member,
        Admin,

    }


}
