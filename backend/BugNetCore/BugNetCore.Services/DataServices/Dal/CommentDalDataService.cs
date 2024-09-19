namespace BugNetCore.Services.DataServices.Dal
{

    public class CommentDalDataService : BaseDalDataService<Comment, CommentDalDataService>, ICommentDataService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IUserNotificationRepo _userNotificationRepo;

        public CommentDalDataService(
            INotificationRepo notificationRepo,
            IUserNotificationRepo userNotificationRepo,
            ICommentRepo mainRepo,
            IAppLogging<CommentDalDataService> logger) : base(mainRepo, logger)
        {
            _notificationRepo = notificationRepo;
            _userNotificationRepo = userNotificationRepo;
        }



        public async Task<Comment> AddAndNotifyAsync(Comment entity, NotificationCallback notificationCallBack, bool persist = true)
        {
            var result = await _mainRepo.AddAsync(entity, persist);

            var comment = entity;

            if (result != 0)
            {
                // Get the newly added bug
                comment = await _mainRepo.FindAsync(entity.Id);

                // Create a new notification
                var notification = new Notification
                {
                    Type = NotificationType.Comment,
                    Message = $"{comment.Sender.Username} added a comment on a bug in the {comment.Bug.Project.Name} project",
                    BugId = comment.BugId,
                };

                // Add the notification to the database, if successfull create NEW unread UserNotification for the customer and all admins and attach it to the notification
                var notificationResult = await _notificationRepo.AddAsync(notification);
                if (notificationResult != 0)
                {
                    // Get the ids for all the users to notify  (the customer of the bug or the assigned developer) based on the sender
                    var userIdsToNotify = new List<Guid>();
                    if (comment.SenderId == comment.Bug.CustomerId && comment.Bug.DevId != null)
                    {
                        userIdsToNotify.Add(comment.Bug.DevId.Value);
                    }
                    else
                    {
                        userIdsToNotify.Add(comment.Bug.CustomerId);
                    }

                    // Create a new UserNotification for the users to notify
                    foreach (var id in userIdsToNotify)
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
                                CreatedAt = notification.CreatedAt,
                                AdditionalInfo = new Dictionary<string, Guid>
                                {
                                    { "BugId", comment.BugId }
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
                throw new Exception("Failed to add the comment");
            }

            return comment;

        }
    }
}