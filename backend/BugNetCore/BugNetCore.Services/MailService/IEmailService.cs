namespace BugNetCore.Services.MailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailWithTemplateAsync<TModel>(string to, string subject, string templateName, TModel model);
        Task SendEmailVerificationEmailAsync(string userName, string to, string verificationToken);
        Task SendPasswordResetEmailAsync(string userName, string to, string resetToken);
    }
}
