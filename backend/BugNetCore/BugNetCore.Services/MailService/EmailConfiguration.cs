using BugNetCore.Services.MailService.Settings;

namespace BugNetCore.Services.MailService
{
    public static class EmailConfigurations
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("EmailSettings").Get<EmailSettings>());
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
