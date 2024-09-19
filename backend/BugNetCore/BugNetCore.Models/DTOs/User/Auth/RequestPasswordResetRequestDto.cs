namespace BugNetCore.Models.DTOs.User.Auth
{
    public class RequestPasswordResetRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}