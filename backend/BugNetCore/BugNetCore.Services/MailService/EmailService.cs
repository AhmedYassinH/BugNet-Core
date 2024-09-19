
using BugNetCore.Services.MailService.Models;
using BugNetCore.Services.MailService.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RazorLight;

namespace BugNetCore.Services.MailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly RazorLightEngine _razorEngine;
        private readonly JwtOptions _jwtOptions;

        public EmailService(
            EmailSettings emailSettings,
            JwtOptions jwtOptions
            )
        {
            _emailSettings = emailSettings; 
            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailService", "Templates"))
                .UseMemoryCachingProvider()
                .Build();
            _jwtOptions = jwtOptions; 
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer,
                int.Parse(_emailSettings.Port), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username,
                _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailWithTemplateAsync<TModel>(string to, string subject, string templateName, TModel model)
        {
            string result = await _razorEngine.CompileRenderAsync(templateName, model);
            await SendEmailAsync(to, subject, result, true);
        }

        public async Task SendEmailVerificationEmailAsync(string userName, string to, string verificationToken)
        {
            var model = new EmailVerificationModel
            {
                UserName = userName,
                VerificationLink = $"{_jwtOptions.EmailVerificationRoute}{verificationToken}",
            };

            await SendEmailWithTemplateAsync(to, "Verify Your Email Address", "EmailVerificationTemplate", model);
        }

        public async Task SendPasswordResetEmailAsync(string userName, string to, string resetToken)
        {
            var model = new PasswordResetVerificationModel
            {
                UserName = userName,
                VerificationLink = $"{_jwtOptions.PasswordResetVerificationRoute}{resetToken}",
            };

            await SendEmailWithTemplateAsync(to, "Reset Your Password", "PasswordResetVerificationTemplate", model);
        }
    }

}
