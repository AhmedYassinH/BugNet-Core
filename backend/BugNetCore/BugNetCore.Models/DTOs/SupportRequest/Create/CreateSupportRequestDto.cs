namespace BugNetCore.Models.DTOs.SupportRequest
{
    public class CreateSupportRequestDto
    {
        [Required]
        public Guid BugId { get; set; } // FK

        public SupportRequestStatus Status { get; set; }

    }

}
