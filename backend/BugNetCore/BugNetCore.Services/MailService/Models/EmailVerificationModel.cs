namespace BugNetCore.Services.MailService.Models;

public class EmailVerificationModel
{
    public string UserName { get; set; }
    public string VerificationLink { get; set; }
}