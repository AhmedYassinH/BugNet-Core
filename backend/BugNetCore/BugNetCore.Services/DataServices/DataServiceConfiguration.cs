namespace BugNetCore.Services.DataServices
{
    public static class DataServiceConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IProjectRepo, ProjectRepo>();
            services.AddScoped<IBugRepo, BugRepo>();
            services.AddScoped<ICommentRepo, CommentRepo>();
            services.AddScoped<ISupportRequestRepo, SupportRequestRepo>();
            services.AddScoped<IChatRoomRepo, ChatRoomRepo>();
            services.AddScoped<IChatMessageRepo, ChatMessageRepo>();
            services.AddScoped<INotificationRepo, NotificationRepo>();
            services.AddScoped<IUserNotificationRepo, UserNotificationRepo>();

            return services;
        }
        public static IServiceCollection AddDataServices(
        this IServiceCollection services)
        {

            services.AddScoped<IUserDataService, UserDalDataService>();
            services.AddScoped<IBugDataService, BugDalDataService>();
            services.AddScoped<ISupportRequestDataService, SupportRequestDalDataService>();
            services.AddScoped<INotificationDataService, NotificationDalDataService>();
            services.AddScoped<ICommentDataService, CommentDalDataService>();

            return services;
        }

    }
}
