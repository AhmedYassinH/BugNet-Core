namespace BugNetCore.Models.DTOs.Bug
{
    public class UpdateBugRequestDto : BaseEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public BugCategory Category { get; set; }

        [Required]
        public BugSeverity CustomerAssignedSeverity { get; set; }
        
        public BugPriority? AdminAssignedPriority { get; set; }


        [Required]
        public BugStatus Status { get; set; }

        public IFormFile? ScreenshotFile { get; set; }

        [Required]
        public Guid ProjectId { get; set; } // FK

      
        public Guid? CustomerId { get; set; } // Fk

        public Guid? DevId { get; set; } // Fk

    }
}
