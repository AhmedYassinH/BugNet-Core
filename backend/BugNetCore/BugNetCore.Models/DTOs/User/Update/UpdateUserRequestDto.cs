namespace BugNetCore.Models.DTOs.User
{
    public class UpdateUserRequestDto : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Role UserRole { get; set; }

        public IFormFile? PictureFile { get; set; }

        public string? Bio { get; set; }

    }
}
