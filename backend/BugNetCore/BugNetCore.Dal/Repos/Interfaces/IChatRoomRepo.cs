namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface IChatRoomRepo : IBaseRepo<ChatRoom>
    {
        Task<ChatRoom> FindBySupportRequestIdAsNoTrackingAsync(Guid SupportRequestId);


    }
}