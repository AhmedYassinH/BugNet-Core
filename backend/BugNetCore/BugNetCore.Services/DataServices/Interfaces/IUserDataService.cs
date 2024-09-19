namespace BugNetCore.Services.DataServices.Interfaces
{
    public interface IUserDataService : IBaseDataService<User>
    {
        Task<User> FindByEmailAsNoTrackingAsync(string email);
        Task<AuthResponseDto> LoginUserAsync(LoginUserRequestDto userDTO);
        Task<bool> RegisterUserAsync(RegisterUserRequestDto userDTO);
        Task<bool> VerifyEmailAsync(string verificationCode);
        Task<bool> RequestPasswordResetAsync(RequestPasswordResetRequestDto userDTO);
        Task<bool> ConfirmPasswordResetAsync(ConfirmPasswordResetRequestDto userDTO, string resetToken);

    }
}
