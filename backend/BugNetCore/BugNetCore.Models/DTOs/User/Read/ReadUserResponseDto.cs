namespace BugNetCore.Models.DTOs.User
{
    public class ReadUserResponseDto : BaseEntityWithAudit
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public Role UserRole { get; set; }

        public string? Picture { get; set; }

        public string? Bio { get; set; }
    }
}
