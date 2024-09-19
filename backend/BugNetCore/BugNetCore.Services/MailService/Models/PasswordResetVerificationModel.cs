namespace BugNetCore.Services.MailService.Models;

public class PasswordResetVerificationModel
{
    public string UserName { get; set; }
    public string VerificationLink { get; set; }
}