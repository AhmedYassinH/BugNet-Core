


namespace BugNetCore.Dal.Repos
{
    public class NotificationRepo : BaseRepo<Notification>, INotificationRepo
    {
        private readonly DbSet<UserNotification> _userNotificationTable;

        public NotificationRepo(ApplicationDbContext context) : base(context)
        {
            _userNotificationTable = context.Set<UserNotification>();
        }

        public IEnumerable<Notification> GetAllByUserId(Guid userId)
        {
            return _userNotificationTable
                .Include(x => x.Notification)
                .ThenInclude(x => x.UserNotifications)
                .Where(x => x.UserId == userId)
                .Select(x => x.Notification)
                .ToList();
        }

        public IEnumerable<Notification> GetOneByIdAndUserId(Guid notificationId, Guid userId)
        {
            return _userNotificationTable
                .Include(x => x.Notification)
                .ThenInclude(x => x.UserNotifications)
                .Where(x => x.UserId == userId && x.NotificationId == notificationId)
                .Select(x => x.Notification)
                .ToList();
        }
    }
}
