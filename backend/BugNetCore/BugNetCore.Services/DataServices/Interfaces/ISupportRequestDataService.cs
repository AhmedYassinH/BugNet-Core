namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface ISupportRequestDataService : IBaseDataService<SupportRequest>
    {
        Task<UpdateSupportResponseDto> HandleSupportRequestActionAndNotifyAsync(ActOnSupportRequestDto actOnSupportRequestDto, NotificationCallback? notificationCallback = null);
        Task<SupportRequest> AddAndNotifyAsync(SupportRequest entity, NotificationCallback notificationCallBack, bool persist = true);

    }
}
