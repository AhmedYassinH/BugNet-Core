
using BugNetCore.Models.DTOs.Notification;

namespace BugNetCore.Services.DataServices.Dal
{

    public class NotificationDalDataService : BaseDalDataService<Notification, NotificationDalDataService>, INotificationDataService
    {
        private readonly IUserNotificationRepo _userNotificationRepo;

        public NotificationDalDataService(
            IUserNotificationRepo userNotificationRepo,
            INotificationRepo mainRepo,
            IAppLogging<NotificationDalDataService> logger) : base(mainRepo, logger)
        {
            _userNotificationRepo = userNotificationRepo;
        }

        public async Task<IEnumerable<ReadNotificationMinimalResponseDto>> GetAllUserNotificationsAsync(Guid userId)
        {
            var notifications = ((INotificationRepo)_mainRepo).GetAllByUserId(userId);

            var response = notifications.Select(x =>
            {

                var notificationRes = new ReadNotificationMinimalResponseDto
                {
                    NotificationId = x.Id,
                    Type = x.Type,
                    Message = x.Message,
                    IsRead = x.UserNotifications.Where(y => y.UserId == userId).FirstOrDefault().IsRead,
                    CreatedAt = x.CreatedAt,
                    AdditionalInfo = new Dictionary<string, Guid>()
                };

                if (x.Type == NotificationType.SupportRequest || x.Type == NotificationType.ChatInvitation)
                {
                    notificationRes.AdditionalInfo.Add("SupportRequestId", (Guid)x.SupportRequestId);

                }
                else if (x.BugId != null)
                {
                    notificationRes.AdditionalInfo.Add("BugId", (Guid)x.BugId);

                }

                return notificationRes;

            }).Where(n => n.AdditionalInfo.Count != 0);

            return response;

        }

        public async Task ReadNotificationAsync(Guid notificationId, Guid userId)
        {
            var userNotification = await _userNotificationRepo.FindByUserIdAndNotificationIdAsync(userId: userId, notificationId: notificationId);

            if (userNotification != null)
            {
                var rows = await _userNotificationRepo.MarkAsReadAsync(userNotification.Id);

                if (rows == 0)
                {
                    throw new Exception("Failed to mark notification as read");
                }
            }
        }
    }
}