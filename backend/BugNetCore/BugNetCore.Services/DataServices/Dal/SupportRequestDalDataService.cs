

using BugNetCore.Models.DTOs.Bug;
using BugNetCore.Models.DTOs.User;

namespace BugNetCore.Services.DataServices.Dal
{
    public class SupportRequestDalDataService : BaseDalDataService<SupportRequest, SupportRequestDalDataService>, ISupportRequestDataService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IChatRoomRepo _chatRoomRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly IUserNotificationRepo _userNotificationRepo;

        public SupportRequestDalDataService(
            IUserNotificationRepo userNotificationRepo,
            INotificationRepo notificationRepo,
            ISupportRequestRepo mainRepo,
            IUserRepo userRepo,
            IChatRoomRepo chatRoomRepo,
            IAppLogging<SupportRequestDalDataService> logger,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper) : base(mainRepo, logger)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _chatRoomRepo = chatRoomRepo;
            _notificationRepo = notificationRepo;
            _userNotificationRepo = userNotificationRepo;
        }



        public async Task<SupportRequest> AddAndNotifyAsync(SupportRequest entity, NotificationCallback notificationCallBack, bool persist = true)
        {
            try
            {
                await _mainRepo.AddAsync(entity, persist);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


            // Get the customerId
            Guid customerId = await ((ISupportRequestRepo)_mainRepo).GetCustomerIdBySupportRequestIdAsync(entity.Id);
            // Get the customer
            User customer = await _userRepo.FindAsync(customerId);

            // Create a new notification
            var notification = new Notification
            {
                Type = NotificationType.SupportRequest,
                Message = $"{customer.Username} has requested an urgent support, Please click here to view the request",
                SupportRequestId = entity.Id
            };

            try
            {

                await _notificationRepo.AddAsync(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            // Create a new UserNotification for all admins
            var admins = await _userRepo.GetAdminIdsAsync();
            var userNotificationsToAdd = admins.Select(adminId => new UserNotification
            {
                NotificationId = notification.Id,
                UserId = adminId,
                IsRead = false
            });

            try
            {
                await _userNotificationRepo.AddRangeAsync(userNotificationsToAdd);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

            // Notify the admins
            foreach (var userNotification in userNotificationsToAdd)
            {
                await notificationCallBack(userNotification.UserId, new ReadNotificationMinimalResponseDto
                {
                    NotificationId = notification.Id,
                    Type = notification.Type,
                    Message = notification.Message,
                    IsRead = userNotification.IsRead,
                    CreatedAt = notification.CreatedAt,
                    AdditionalInfo = new Dictionary<string, Guid>
                    {
                        { "SupportRequestId", entity.Id }
                    }
                });
            }

            return entity;
        }



        public async Task<ReadSupportResponseDto> FindWithUsersAsync(Guid id)
        {
            var request = await FindAsync(id);
            var bugWithoutCustomer = _mapper.Map<Bug>(request.Bug);
            bugWithoutCustomer.Customer = null;
            var response = new ReadSupportResponseDto
            {
                Id = request.Id,
                Bug = _mapper.Map<ReadBugResponseDto>(bugWithoutCustomer),
                Customer = _mapper.Map<ReadUserResponseDto>(request.Bug.Customer),
                SupportDev = _mapper.Map<ReadUserResponseDto>(request.ChatRoom?.SupportDev),
                Status = request.Status,
                BugId = request.BugId,
                RowVersion = request.RowVersion

            };

            return response;
        }

        public async Task<IEnumerable<ReadSupportResponseDto>> GetAllWithUsersAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAscending, int pageSize, int pageNumber)
        {
            IEnumerable<SupportRequest> requests = await GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageSize, pageNumber);

            var response = requests.Select(supportRequest =>
            {
                var bugWithoutCustomer = _mapper.Map<Bug>(supportRequest.Bug);
                bugWithoutCustomer.Customer = null;
                return new ReadSupportResponseDto
                {
                    Id = supportRequest.Id,
                    Bug = _mapper.Map<ReadBugResponseDto>(bugWithoutCustomer),
                    Customer = _mapper.Map<ReadUserResponseDto>(supportRequest.Bug.Customer),
                    SupportDev = _mapper.Map<ReadUserResponseDto>(supportRequest.ChatRoom?.SupportDev),
                    Status = supportRequest.Status,
                    BugId = supportRequest.BugId,
                    RowVersion = supportRequest.RowVersion

                };

            });
            return response;

        }

        public async Task<IEnumerable<ReadSupportResponseDto>> GetAllRequestsByUserAsync(Guid userId, string? filterOn, string? filterQuery, string? sortBy, bool isAscending, int pageSize, int pageNumber)
        {
            IEnumerable<SupportRequest> requests = await GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageSize, pageNumber);

            // For each one retrieved make sure the userId either belong to the Customer, Supporting dev 
            requests = requests.Where(request => request.Bug.CustomerId == userId || request.ChatRoom?.SupportDevId == userId);

            var response = requests.Select(supportRequest =>
            {
                var bugWithoutCustomer = _mapper.Map<Bug>(supportRequest.Bug);
                bugWithoutCustomer.Customer = null;
                return new ReadSupportResponseDto
                {
                    Id = supportRequest.Id,
                    Bug = _mapper.Map<ReadBugResponseDto>(bugWithoutCustomer),
                    Customer = _mapper.Map<ReadUserResponseDto>(supportRequest.Bug.Customer),
                    SupportDev = _mapper.Map<ReadUserResponseDto>(supportRequest.ChatRoom?.SupportDev),
                    Status = supportRequest.Status,
                    BugId = supportRequest.BugId,
                    RowVersion = supportRequest.RowVersion

                };

            });
            return response;
        }

       

        public async Task<UpdateSupportResponseDto> HandleSupportRequestActionAndNotifyAsync(ActOnSupportRequestDto actOnSupportRequestDto, NotificationCallback? notificationCallback = null)
        {
            // Instantiate the response DTO
            UpdateSupportResponseDto updateSupportResponseDto = new UpdateSupportResponseDto();

            // Check if the action is user or admin action
            bool isAdminAction = actOnSupportRequestDto.Action != SupportRequestAction.Cancel;


            // Get the user from access token
            // User userFromToken = await GetUserFromTokenAsync();

            // if (isAdminAction && userFromToken.UserRole != Role.Admin)
            // {
            //     // throw new BorrowingActionForbiddenException($"Only an admin can perform this action!");
            //     throw new Exception();
            // }

            SupportRequest supportRequest = await _mainRepo.FindAsync(actOnSupportRequestDto.Id);
            if (supportRequest == null)
            {
                // throw new SupportRequestNotFoundException($"Support Request with id {actOnSupportRequestDto.Id} does not exist");
                throw new Exception();
            }

            if (actOnSupportRequestDto.Action == SupportRequestAction.Approve)
            {
                // Check if SupportDevId is null
                if (actOnSupportRequestDto.SupportDevId == null)
                {
                    // throw new SupportDevIdNullException("SupportDevId cannot be null when approving a support request");
                    throw new Exception();
                }

                // Check if the support dev exists
                User? supportDev = await _userRepo.FindAsync(actOnSupportRequestDto.SupportDevId.Value);
                if (supportDev == null)
                {
                    // throw new SupportDevNotFoundException($"Support Developer with id {actOnSupportRequestDto.SupportDevId} does not exist");
                    throw new Exception();
                }

                // Check if the support dev is a support dev
                if (supportDev.UserRole != Role.Dev)
                {
                    // throw new SupportDevRoleException($"User with id {actOnSupportRequestDto.SupportDevId} is not a support developer");
                    throw new Exception();
                }

                // Find the chat room for the support request and assign the support dev to it, if room soesn't exist, create it
                ChatRoom? chatRoom = await _chatRoomRepo.FindBySupportRequestIdAsNoTrackingAsync(actOnSupportRequestDto.Id);
                if (chatRoom == null)
                {
                    chatRoom = new ChatRoom
                    {
                        SupportRequestId = actOnSupportRequestDto.Id,
                        SupportDevId = supportDev.Id
                    };
                    await _chatRoomRepo.AddAsync(chatRoom);
                }
                else
                {
                    chatRoom.SupportDevId = supportDev.Id;
                    await _chatRoomRepo.UpdateAsync(chatRoom);
                }

                // Create a new notification, then and two UserNotifications for the customer and the support dev
                Notification notification = new Notification
                {
                    Type = NotificationType.ChatInvitation,
                    Message = $"A requested Support request has been approved. Please click here to start chatting.",
                    SupportRequestId = actOnSupportRequestDto.Id
                };

                await _notificationRepo.AddAsync(notification);

                Guid customerId = await ((ISupportRequestRepo)_mainRepo).GetCustomerIdBySupportRequestIdAsync(actOnSupportRequestDto.Id);
                List<Guid> userIdsToNotify = new List<Guid> { customerId, supportDev.Id };
                var userNotificationsToAdd = userIdsToNotify.Select(id => new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = id,
                    IsRead = false
                });
                await _userNotificationRepo.AddRangeAsync(userNotificationsToAdd);

                // Call the notification callback if provided
                if (notificationCallback != null)
                {
                    foreach (var userNotification in userNotificationsToAdd)
                    {
                        await notificationCallback(userNotification.UserId, new ReadNotificationMinimalResponseDto
                        {
                            NotificationId = notification.Id,
                            Type = notification.Type,
                            Message = notification.Message,
                            IsRead = userNotification.IsRead,
                            CreatedAt = notification.CreatedAt,
                            AdditionalInfo = new Dictionary<string, Guid>
                            {
                                { "SupportRequestId", actOnSupportRequestDto.Id }
                            }
                        });
                    }
                }


                // Update the support request status
                supportRequest.Status = SupportRequestStatus.Approved;
                await _mainRepo.UpdateAsync(supportRequest);

            }
            else
            {

                // Update the support request status
                // TODO: replace this with untracked upadate cause this violates concurrency
                supportRequest.Status = actOnSupportRequestDto.Action switch
                {
                    SupportRequestAction.Reject => SupportRequestStatus.Rejected,
                    SupportRequestAction.Cancel => SupportRequestStatus.Canceled,
                    SupportRequestAction.Close => SupportRequestStatus.Closed,
                    _ => throw new Exception()
                };
                await _mainRepo.UpdateAsync(supportRequest);

            }

            // map to the reposne from the support request entity
            updateSupportResponseDto = _mapper.Map<UpdateSupportResponseDto>(supportRequest);

            return updateSupportResponseDto;


        }



        private async Task<User> GetUserFromTokenAsync()
        {
            // Get the user id from token and fetch it for later
            ClaimsIdentity identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            Guid userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Fetch user 
            User userFromToken = await _userRepo.FindAsync(userId);
            // if (userFromToken == null)
            // {
            //     throw new BorrowingUserNotFoundException($"User with id {userId} does not exist");
            // }

            return userFromToken;
        }
    }
}