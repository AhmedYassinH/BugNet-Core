
namespace BugNetCore.Dal.Repos
{
    public class ChatRoomRepo : BaseRepo<ChatRoom>, IChatRoomRepo
    {
        public ChatRoomRepo(ApplicationDbContext context) : base(context)
        {
        }


        public Task<ChatRoom> FindBySupportRequestIdAsNoTrackingAsync(Guid SupportRequestId)
        {
            return Table
                .AsNoTracking()
                .Include(x => x.SupportRequest)
                .ThenInclude(x => x.Bug)
                .FirstOrDefaultAsync(x => x.SupportRequestId == SupportRequestId);
        }

    }
}