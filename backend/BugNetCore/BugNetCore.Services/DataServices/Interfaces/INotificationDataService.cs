namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface INotificationDataService : IBaseDataService<Notification>
    {
        Task<IEnumerable<ReadNotificationMinimalResponseDto>> GetAllUserNotificationsAsync(Guid userId);

        Task ReadNotificationAsync(Guid notificationId, Guid userId);

    }
}
