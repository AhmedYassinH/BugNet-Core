
namespace BugNetCore.Dal.Repos
{
    public class ChatMessageRepo : BaseRepo<ChatMessage>, IChatMessageRepo
    {
        public ChatMessageRepo(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<ChatMessage> GetAllByChatRoomId(Guid ChatRoomId)
        {
            return Table
            .Where(x => x.ChatRoomId == ChatRoomId)
            .Include(x => x.Sender)
            .ToList();
        }
    }
}