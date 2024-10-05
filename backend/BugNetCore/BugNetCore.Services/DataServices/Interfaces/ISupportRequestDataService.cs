namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface ISupportRequestDataService : IBaseDataService<SupportRequest>
    {

        Task<IEnumerable<ReadSupportResponseDto>> GetAllWithUsersAsync(
            string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending,
            int pageSize, int pageNumber);
        Task<ReadSupportResponseDto> FindWithUsersAsync(Guid id);
        Task<UpdateSupportResponseDto> HandleSupportRequestActionAndNotifyAsync(ActOnSupportRequestDto actOnSupportRequestDto, NotificationCallback? notificationCallback = null);
        Task<SupportRequest> AddAndNotifyAsync(SupportRequest entity, NotificationCallback notificationCallBack, bool persist = true);

    }
}
