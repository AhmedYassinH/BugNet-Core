
using BugNetCore.Models.DTOs.Notification;

namespace BugNetCore.Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
        }

        public async Task SendNotificationToUserAsync(Guid userId, ReadNotificationMinimalResponseDto notification)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
        }
    }
}