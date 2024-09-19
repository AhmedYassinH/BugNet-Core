namespace BugNetCore.Dal.Repos.Interfaces
{
    public interface IChatMessageRepo : IBaseRepo<ChatMessage>
    {
        IEnumerable<ChatMessage> GetAllByChatRoomId(Guid ChatRoomId);

    }
}