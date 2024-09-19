using BugNetCore.Services.MailService;
using BugNetCore.Services.MailService.Settings;

namespace BugNetCore.Services.DataServices.Dal
{

    public class UserDalDataService : BaseDalDataService<User, UserDalDataService>, IUserDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly EmailSettings _emailSettings;
        private readonly JwtOptions _jwtOptions;

        public UserDalDataService(
            IEmailService emailService,
            EmailSettings emailSettings,
            JwtOptions jwtOptions,
            IHttpContextAccessor httpContextAccessor,
            IUserRepo mainRepo,
            IAppLogging<UserDalDataService> logger) : base(mainRepo, logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _emailSettings = emailSettings;
            _jwtOptions = jwtOptions;
        }


        public async Task<User> FindByEmailAsNoTrackingAsync(string email)
        {
            return await ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(email);
        }

        public async Task<AuthResponseDto> LoginUserAsync(LoginUserRequestDto userDTO)
        {
            // check if a user with this email exists
            User user = await ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(userDTO.Email);

            if (user == null)
            {
                _logger.LogAppWarning("Invalid email!");
                // throw new InvalidUserException();
                throw new Exception("Invalid email!");
            }

            // check if user's email is verified
            if (!user.IsVerified)
            {
                _logger.LogAppWarning("Email not verified!");
                // throw new EmailNotVerifiedException();
                throw new Exception("Email not verified!");
            }

            // check if the provided password is correct
            bool passwordIsValid = JwtHelpers.VerifyPasswordHash(userDTO.Password, user.PasswordHash);

            if (!passwordIsValid)
            {
                _logger.LogAppWarning("Invalid Password");
                // throw new InvalidUserException();
                throw new Exception("Invalid Password");
            }

            // Generate Jwt token 
            string accessToken = JwtHelpers.GenerateJwtToken(user, _jwtOptions);
            AuthResponseDto authResponse = new AuthResponseDto
            {
                AccessToken = accessToken,
                UserId = user.Id,
                UserName = user.Username,
                UserRole = user.UserRole,
                ImageUrl = user.Picture,
            };

            return authResponse;

        }

        public async Task<bool> RegisterUserAsync(RegisterUserRequestDto userDTO)
        {

            // check if a user with this email already exists
            User user = await ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(userDTO.Email);

            if (!(user == null))
            {
                _logger.LogAppWarning("User with the same email already exists");
                // throw new UserAlreadyExistException();
                throw new Exception("User with the same email already exists");
            }

            // Default image
            string imageName = "User.jpg";

            var imageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/Users/{imageName}";


            // Map the DTO to a user
            user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = JwtHelpers.HashPassword(userDTO.Password),
                UserRole = Role.Customer,
                Picture = imageUrl,
            };


            // Add the new user to the database
            await _mainRepo.AddAsync(user);
            await _mainRepo.SaveChangesAsync();

            // generate email verification token from the user's email
            string verificationToken = JwtHelpers.GenerateJwtToken(user, _jwtOptions);

            // send the verification email
            await _emailService.SendEmailVerificationEmailAsync(user.Username, user.Email, verificationToken);

            
            return true;

        }

        public async Task<bool> RequestPasswordResetAsync(RequestPasswordResetRequestDto userDTO)
        {
            // Check if a user with this email exists
            User user = await ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(userDTO.Email);

            if (user == null || !user.IsVerified)
            {
                _logger.LogAppWarning("Invalid email!");
                // throw new InvalidUserException();
                throw new Exception("Invalid email!");
            }

            // Check if the user is verified
            if (!user.IsVerified)
            {
                _logger.LogAppWarning("Email not verified!");
                // throw new EmailNotVerifiedException();
                throw new Exception("Email not verified!");
            }

            // Generate password reset token
            string resetToken = JwtHelpers.GenerateJwtToken(user, _jwtOptions);

            // Send the password reset email
            await _emailService.SendPasswordResetEmailAsync(user.Username, user.Email, resetToken);

            return true;
        }

        public async Task<bool> ConfirmPasswordResetAsync(ConfirmPasswordResetRequestDto userDTO, string resetToken)
        {
            try
            {
                User validUser = JwtHelpers.ValidateJwtToken(resetToken, _jwtOptions);
                User user = ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(validUser.Email).Result;

                if (user == null)
                {
                    _logger.LogAppWarning("Invalid email!");
                    // throw new InvalidUserException();
                    throw new Exception("Invalid email!");
                }

                
                user.PasswordHash = JwtHelpers.HashPassword(userDTO.NewPassword);
                await _mainRepo.UpdateAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> VerifyEmailAsync(string verificationCode)
        {
            try{
                User validUser = JwtHelpers.ValidateJwtToken(verificationCode, _jwtOptions);
                User user = await ((IUserRepo)_mainRepo).FindByEmailAsNoTrackingAsync(validUser.Email);
                if (user != null && !user.IsVerified)
                {
                    user.IsVerified = true;
                    await _mainRepo.UpdateAsync(user);
                    return true;
                }
                return false;


            }   
            catch (Exception)
            {
                return false;
            }

        }

      
    }
}