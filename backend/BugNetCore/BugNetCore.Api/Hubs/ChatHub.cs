using BugNetCore.Models.DTOs.ChatMessage;
using BugNetCore.Models.DTOs.ChatRoom;
using Microsoft.AspNetCore.SignalR;
namespace BugNetCore.Api.Hubs
{
    public class ChatHub : Hub
    {
        // create constructor and except the db context, the chat room repository and the chat message repository
        private readonly IChatRoomRepo _chatRoomRepo;
        private readonly IChatMessageRepo _chatMessageRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public ChatHub(
            IChatRoomRepo chatRoomRepo,
             IChatMessageRepo chatMessageRepo,
             IUserRepo userRepo,
             IMapper mapper)
        {
            _chatRoomRepo = chatRoomRepo;
            _chatMessageRepo = chatMessageRepo;
            _userRepo = userRepo;

            _mapper = mapper;
        }

        public async Task<List<ReceiveMessageResponseDto>> JoinRoom(JoinRoomRequestDto request)
        {

            List<ReceiveMessageResponseDto> messages = new List<ReceiveMessageResponseDto>();
            // Check if a chat room exists with the given support request id
            var chatRoom = await _chatRoomRepo.FindBySupportRequestIdAsNoTrackingAsync(request.SupportRequestId);
            if (chatRoom == null)
            {
                await Clients.Caller.SendAsync("RoomNotFound", request.SupportRequestId);
                return messages;
            }

            // Check if the user is the SupportDevId of the chat room or the the Customer associated with the Bug that the support request is for
            if (chatRoom.SupportDevId != request.UserId && chatRoom.SupportRequest.Bug.CustomerId != request.UserId)
            {
                await Clients.Caller.SendAsync("Unauthorized", request.SupportRequestId);
                return messages;
            }

            // Add user to the Group of the Live Support Request
            await Groups.AddToGroupAsync(Context.ConnectionId, request.SupportRequestId.ToString());

            // Finally, send the chat messages to the user
            // _chatMessageRepo.GetAllByChatRoomId(chatRoom.Id).Select(x => x.MessageText).ToList();
            messages = _chatMessageRepo.GetAllByChatRoomId(chatRoom.Id)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new ReceiveMessageResponseDto
            {
                SenderId = x.SenderId,
                SenderName = x.Sender.Username,
                MessageText = x.MessageText,
                SentAt = x.CreatedAt
            })
            .ToList();

            return messages;


        }

        public Task LeaveRoom(LeaveRoomRequestDto request)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, request.SupportRequestId.ToString());
        }

        public async Task SendMessage(SendMessageRequestDto request)
        {
            // TODO: optimize this
            // Check if a chat room exists with the given support request id
            var chatRoom = await _chatRoomRepo.FindBySupportRequestIdAsNoTrackingAsync(request.SupportRequestId);
            if (chatRoom == null)
            {
                await Clients.Caller.SendAsync("RoomNotFound", request.SupportRequestId);
                return;
            }

            // TODO: Check if the user is the SupportDevId of the chat room or the the Customer associated with the Bug that the support request is for
            // Check if the user is the SupportDevId of the chat room or the the Customer associated with the Bug that the support request is for
            // if (chatRoom.SupportDevId != request.UserId && chatRoom.SupportRequest.BugId != request.UserId)
            // {
            //     await Clients.Caller.SendAsync("Unauthorized", request.SupportRequestId);
            //     return;
            // }

            // Create a new chat message
            ChatMessage chatMessage = new ChatMessage
            {
                ChatRoomId = chatRoom.Id,
                MessageText = request.MessageText,
                SenderId = request.SenderId
            };

            // Save the chat message
            await _chatMessageRepo.AddAsync(chatMessage);

            // Get the user name
            var user = await _userRepo.FindAsNoTrackingAsync(chatMessage.SenderId);



            var receiveMessageResponseDto = new ReceiveMessageResponseDto
            {
                SenderId = chatMessage.SenderId,
                SenderName = user.Username,
                MessageText = chatMessage.MessageText,
                SentAt = chatMessage.CreatedAt
            };

            // Send the message to the group (except the sender)
            await Clients.GroupExcept(request.SupportRequestId.ToString(), new[] { Context.ConnectionId }).SendAsync("ReceiveMessage", receiveMessageResponseDto);

        }
        // public async Task GetRoomList()
        // {
        //     // Get all chat rooms
        //     var chatRooms = _chatRoomRepo.GetAllIgnoreQueryFilters();

        //     // Map chat rooms to ReadRoomResponseDto
        //     // var chatRoomDtos = _mapper.Map<List<ReadRoomResponseDto>>(chatRooms);

        //     // Send the chat rooms to the user
        //     // await Clients.Caller.SendAsync("ReceiveRoomList", chatRoomDtos);
        //     // TODO: Map chat rooms to ReadRoomResponseDto
        //     return chatRooms;
        // }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
