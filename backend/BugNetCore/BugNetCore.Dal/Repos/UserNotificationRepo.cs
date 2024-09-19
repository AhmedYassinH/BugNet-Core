
namespace BugNetCore.Dal.Repos
{
    public class UserNotificationRepo : BaseRepo<UserNotification>, IUserNotificationRepo
    {
        public UserNotificationRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserNotification> FindByUserIdAndNotificationIdAsync(Guid userId, Guid notificationId)
        {
            return await Table.FirstOrDefaultAsync(x => x.UserId == userId && x.NotificationId == notificationId);
        }

        public async Task<int> MarkAsReadAsync(Guid id)
        {
            var userNotification = await Table.FindAsync(id);
            
            if (userNotification != null)
            {
                userNotification.IsRead = true;
                return await SaveChangesAsync();
            }
            
            return 0;
            
        }
    }
}
