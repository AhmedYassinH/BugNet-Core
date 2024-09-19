namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface ICommentDataService : IBaseDataService<Comment>
    {
        Task<Comment> AddAndNotifyAsync(Comment entity, NotificationCallback notificationCallBack, bool persist = true);

    }
}
