namespace BugNetCore.Models.Entities
{
    public class User : BaseEntityWithAudit
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        [Column("Role")]
        public Role UserRole { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? Picture { get; set; }
       
        public string? Bio { get; set; }

        [DefaultValue(false)]
        public bool IsVerified { get; set; }


        [InverseProperty(nameof(Bug.Customer))]
        public ICollection<Bug> ReportedBugs { get; set; } = new List<Bug>();

        [InverseProperty(nameof(Bug.Dev))]
        public ICollection<Bug> AssignedBugs { get; set; } = new List<Bug>();

        [InverseProperty(nameof(UserProject.User))]
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

        [InverseProperty(nameof(UserNotification.User))]
        public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

    }

    public enum Role
    {
        Admin,
        Customer,
        Dev,

    }
}