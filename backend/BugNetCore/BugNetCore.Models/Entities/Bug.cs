namespace BugNetCore.Models.Entities
{
    public class Bug : BaseEntityWithAudit
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public BugCategory Category { get; set; }

        [Required]
        public BugSeverity CustomerAssignedSeverity { get; set; }

        public BugPriority? AdminAssignedPriority { get; set; }

        [Required]
        public BugStatus Status { get; set; }

        public string? Screenshot { get; set; }


        [Required]
        public Guid ProjectId { get; set; } // FK

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }



        [Required]
        public Guid CustomerId { get; set; } // Fk

        [ForeignKey(nameof(CustomerId))]
        public User Customer { get; set; }


        public Guid? DevId { get; set; } // Fk

        [ForeignKey(nameof(DevId))]
        public User? Dev { get; set; }


        [InverseProperty(nameof(Comment.Bug))]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [InverseProperty(nameof(SupportRequest.Bug))]
        public SupportRequest SupportRequest { get; set; }

    }


    public enum BugStatus
    {
        Reported,
        InProgress,
        Resolved,
        Testing,

    }

    public enum BugPriority
    {
        Low,
        Medium,
        High,

    }

    public enum BugSeverity
    {
        Urgent,
        High,
        Medium,
        Low,

    }

    public enum BugCategory
    {
        UI,
        Backend,
        Frontend,
        Datbase,
        Other,

    }
}
