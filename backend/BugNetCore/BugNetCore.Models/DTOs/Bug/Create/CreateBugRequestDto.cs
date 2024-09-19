using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace BugNetCore.Models.DTOs.Bug
{
    public class CreateBugRequestDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public BugCategory Category { get; set; }

        [Required]
        public BugSeverity CustomerAssignedSeverity { get; set; }

        [Required]
        public BugStatus Status { get; set; }

        public IFormFile? ScreenshotFile { get; set; }

        [Required]
        public Guid ProjectId { get; set; } // FK

        [JsonIgnore]
        public Guid? CustomerId { get; set; } // Fk

    }

}
