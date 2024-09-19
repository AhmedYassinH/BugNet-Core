using BugNetCore.Models.DTOs.User.Auth;
using BugNetCore.Services.DataServices.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace BugNetCore.Api.Controllers.V1_0_Beta
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0-Beta")]
    public class AuthController : ControllerBase
    {
        private readonly IUserDataService _userDataService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly JwtOptions _jwtOptions;

        public AuthController(IUserDataService userDataService, IDataProtectionProvider dataProtectionProvider, JwtOptions jwtOptions)
        {
            _userDataService = userDataService;
            _dataProtectionProvider = dataProtectionProvider;
            _jwtOptions = jwtOptions;
        }

        [HttpGet("oauth/github-login")]
        public IActionResult GitHubLogin(string returnUrl)
        {
            var state = returnUrl;
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GitHubCallback)),
                Items = { { "state", state } }
            };

            return Challenge(authProperties, "github");
        }



        [HttpGet("oauth/github-cb")]
        public async Task<IActionResult> GitHubCallback()
        {
            var githubState = Request.Cookies["github_state"].Split("|");
            string returnUrl = githubState[1];
            string token = githubState[2];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("GitHub authentication failed");
            }


            // Set the token in an HttpOnly cookie
            Response.Cookies.Append("github_jwt_token", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddSeconds(8),
                Domain = "ahmedyassin.dev",
            });


            // Redirect back to the client app without exposing the token in the URL
            return Redirect(returnUrl);
        }




        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="registerRequestDto">The request DTO containing the user's registration information.</param>
        /// <returns>An ActionResult containing an AuthResponseDto with authentication details upon successful registration.</returns>
        /// <remarks>
        /// This endpoint allows the registration of a new user by processing the provided registration details.
        /// The registration request includes user-specific information such as email, password.
        /// Upon successful registration, the endpoint returns an authentication response DTO containing authentication details
        /// such as jwt access token, expiration time, and user information.
        /// </remarks>
        /// <param name="registerRequestDto">The request DTO containing the user's registration information.</param>
        /// <returns>An ActionResult containing a RegisterUserResponseDto with authentication details upon successful registration.</returns>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto registerRequestDto)
        {

            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);
            }

            bool isRegistered = false;
            try
            {

                isRegistered = await _userDataService.RegisterUserAsync(registerRequestDto);
            }
            // catch (UserAlreadyExistException ex)
            // {
            //     throw new customWebExceptions.ConflictException(ex.Message)
            //     {
            //         Code = "UserConflict"
            //     };
            // }
            catch (UnknownDatabaseException ex)
            {
                throw new customWebExceptions.WebException(ex.Message)
                {
                    Code = "DatabaseError"
                };
            }

            if (isRegistered)
            {
                return Ok(
                    new RegisterUserResponseDto
                    {
                        Message = "User registered successfully. Please verify your email to login."
                    }
                );
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Verifies the registered email of a user after they click the verification link from their email.
        /// </summary>
        /// <param name="verificationCode">The verification code sent to the user's email.</param>
        /// <returns>An ActionResult indicating the result of the verification process.</returns>
        /// <remarks>
        /// This endpoint is used to verify the registered email of a user after they click the verification link from their email.
        /// The verification process requires the user ID and the verification code sent to their email.
        /// Upon successful verification, the endpoint returns an appropriate response indicating the result.
        /// </remarks>
        /// <param name="verificationCode">The verification code sent to the user's email.</param>
        /// <returns>An ActionResult indicating the result of the verification process.</returns>
        [HttpPut]
        [Route("verify/{verificationCode}")]
        public async Task<ActionResult> VerifyEmail([FromRoute] string verificationCode)
        {
            try
            {
                var isSuccess = await _userDataService.VerifyEmailAsync(verificationCode);
                if (isSuccess)
                {
                    return Ok(new { message = "Email verified successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Email verification failed." });
                }

            }
            // catch (InvalidVerificationCodeException ex)
            // {
            //     throw new customWebExceptions.BadRequestException(ex.Message);
            // }
            // catch (UserNotFoundException ex)
            // {
            //     throw new customWebExceptions.NotFoundException(ex.Message);
            // }
            // catch (ExpiredVerificationCodeException ex)
            // {
            //     throw new customWebExceptions.BadRequestException(ex.Message);
            // }
            catch (Exception ex)
            {
                throw new customWebExceptions.WebException(ex.Message);
            }
        }


        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="loginUserRequestDto">The request DTO containing the user's login credentials (email and password).</param>
        /// <returns>An ActionResult containing an AuthResponseDto with authentication details upon successful login.</returns>
        /// <remarks>
        /// This endpoint allows a registered user to log in by providing their email and password.
        /// Upon successful login, the endpoint returns an authentication response DTO containing authentication details
        /// such as jwt access token, expiration time, and user information.
        /// </remarks>
        /// <param name="loginUserRequestDto">The request DTO containing the user's login credentials (email and password).</param>
        /// <returns>An ActionResult containing an AuthResponseDto with authentication details upon successful login.</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginUserRequestDto loginUserRequestDto)
        {


            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);

            }


            AuthResponseDto authResponse;
            // try
            // {

            authResponse = await _userDataService.LoginUserAsync(loginUserRequestDto);
            // }
            // catch (InvalidUserException ex)
            // {
            //     throw new customWebExceptions.UnauthorizedException(ex.Message);
            // }



            return Ok(authResponse);
        }


        /// <summary>
        /// Sends a password reset request to the user's email.
        /// </summary>
        /// <param name="requestPasswordResetRequestDto">The request DTO containing the user's email.</param>
        /// <returns>An ActionResult indicating the result of the password reset request.</returns>
        /// <remarks>
        /// This endpoint allows a user to request a password reset by providing their email.
        /// The endpoint sends a password reset link to the user's email if the email is valid and associated with an existing user.
        /// </remarks>
        /// <param name="requestPasswordResetRequestDto">The request DTO containing the user's email.</param>
        /// <returns>An ActionResult indicating the result of the password reset request.</returns>
        [HttpPost]
        [Route("reset-password/request")]
        public async Task<ActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequestDto requestPasswordResetRequestDto)
        {
            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);

            }

            try
            {
                var isSuccess = await _userDataService.RequestPasswordResetAsync(requestPasswordResetRequestDto);
                if (isSuccess)
                {
                    return Ok(new { message = "Verification link is sent to your email." });
                }
                else
                {
                    return BadRequest(new { message = "Password reset request failed." });
                }
            }
            // catch (UserNotFoundException ex)
            // {
            //     throw new customWebExceptions.NotFoundException(ex.Message);
            // }
            // catch (InvalidEmailException ex)
            // {
            //     throw new customWebExceptions.BadRequestException(ex.Message);
            // }
            catch (Exception ex)
            {
                throw new customWebExceptions.WebException(ex.Message);
            }
        }

        /// <summary>
        /// Confirms the password reset for a user.
        /// </summary>
        /// <param name="confirmPasswordResetRequestDto">The request DTO containing the user's new password and reset token.</param>
        /// <returns>An ActionResult indicating the result of the password reset confirmation.</returns>
        /// <remarks>
        /// This endpoint is used to confirm the password reset for a user.
        /// The confirmation process requires the user's new password and the reset token received through email.
        /// Upon successful confirmation, the endpoint returns an appropriate response indicating the result.
        /// </remarks>
        /// <param name="confirmPasswordResetRequestDto">The request DTO containing the user's new password and reset token.</param>
        /// <returns>An ActionResult indicating the result of the password reset confirmation.</returns>
        [HttpPut]
        [Route("reset-password/confirm/{resetToken}")]
        public async Task<ActionResult> ConfirmPasswordReset([FromBody] ConfirmPasswordResetRequestDto confirmPasswordResetRequestDto, [FromRoute] string resetToken)
        {

            if (!ModelState.IsValid)
            {

                Dictionary<string, string[]> errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(y => y.ErrorMessage).ToArray());

                throw new customWebExceptions.ValidationException(errors);

            }
            try
            {
                var isSuccess = await _userDataService.ConfirmPasswordResetAsync(confirmPasswordResetRequestDto, resetToken);
                if (isSuccess)
                {
                    return Ok(new { message = "Password changed successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Password reset confirmation failed." });
                }
            }
            // catch (InvalidResetTokenException ex)
            // {
            //     throw new customWebExceptions.BadRequestException(ex.Message);
            // }
            // catch (ExpiredResetTokenException ex)
            // {
            //     throw new customWebExceptions.BadRequestException(ex.Message);
            // }
            // catch (UserNotFoundException ex)
            // {
            //     throw new customWebExceptions.NotFoundException(ex.Message);
            // }
            catch (Exception ex)
            {
                throw new customWebExceptions.WebException(ex.Message);
            }
        }


    }
}