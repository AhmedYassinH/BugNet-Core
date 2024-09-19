
namespace BugNetCore.Models.DTOs.User.Auth
{
    public class AuthResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Role UserRole { get; set; }
        public string? ImageUrl { get; set; }
        public string AccessToken { get; set; }

    }
}