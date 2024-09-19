namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface IBugDataService : IBaseDataService<Bug>
    {
        Task<Bug> AddAndNotifyAsync(Bug entity, NotificationCallback notificationCallBack, bool persist = true);

    }
}
