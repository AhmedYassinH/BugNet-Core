namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface IUserNotificationRepo : IBaseRepo<UserNotification>
    {
        Task<UserNotification> FindByUserIdAndNotificationIdAsync(Guid userId, Guid notificationId);
        Task<int> MarkAsReadAsync(Guid id);

    }
}
