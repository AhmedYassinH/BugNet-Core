namespace BugNetCore.Services.DataServices.Dal
{

    public class BugDalDataService : BaseDalDataService<Bug, BugDalDataService>, IBugDataService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IUserNotificationRepo _userNotificationRepo;
        private readonly IUserRepo _userRepo;

        public BugDalDataService(
            IUserRepo userRepo,
            INotificationRepo notificationRepo,
            IUserNotificationRepo userNotificationRepo,
            IBugRepo mainRepo,
            IAppLogging<BugDalDataService> logger) : base(mainRepo, logger)
        {
            _notificationRepo = notificationRepo;
            _userNotificationRepo = userNotificationRepo;
            _userRepo = userRepo;
        }



        public async Task<Bug> AddAndNotifyAsync(Bug entity, NotificationCallback notificationCallBack, bool persist = true)
        {
            var result = await _mainRepo.AddAsync(entity, persist);

            var bug = entity;

            if (result != 0)
            {
                // Get the newly added bug
                bug = await _mainRepo.FindAsync(entity.Id);

                // Create a new notification
                var notification = new Notification
                {
                    Type = NotificationType.BugCreation,
                    Message = $"{bug.Customer.Username} has reported a new bug",
                    BugId = bug.Id,
                };

                // Add the notification to the database, if successfull create NEW unread UserNotification for the customer and all admins and attach it to the notification
                var notificationResult = await _notificationRepo.AddAsync(notification);
                if (notificationResult != 0)
                {
                    // Get the ids for all the admins to notify them
                    var adminsIdsToNotify = (List<Guid>)await _userRepo.GetAdminIdsAsync();

                    // Create a new UserNotification for the users to notify
                    foreach (var id in adminsIdsToNotify)
                    {
                        var userNotification = new UserNotification
                        {
                            NotificationId = notification.Id,
                            UserId = id,
                            IsRead = false
                        };

                        // Add the UserNotification to the database
                        var userNotificationResult = await _userNotificationRepo.AddAsync(userNotification);

                        if (userNotificationResult != 0)
                        {
                            // Call the notification callback
                            await notificationCallBack(id, new ReadNotificationMinimalResponseDto
                            {
                                NotificationId = notification.Id,
                                Type = notification.Type,
                                Message = notification.Message,
                                IsRead = userNotification.IsRead,
                                AdditionalInfo = new Dictionary<string, Guid>
                                {
                                    { "BugId", bug.Id }
                                }
                            });
                        }

                        else
                        {
                            throw new Exception("Failed to add the user notification");
                        }
                    }

                }
            }
            else
            {
                throw new Exception("Failed to add the bug");
            }

            return bug;

        }
    }
}