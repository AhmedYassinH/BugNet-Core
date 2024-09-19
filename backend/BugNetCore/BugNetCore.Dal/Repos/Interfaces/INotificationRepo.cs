namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface INotificationRepo : IBaseRepo<Notification>
    {
        IEnumerable<Notification> GetAllByUserId(Guid userId);

        IEnumerable<Notification> GetOneByIdAndUserId(Guid notificationId, Guid userId);

    }
}
