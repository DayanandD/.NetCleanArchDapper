using NETCleanArch.Application.Dtos.Requests;
using NETCleanArch.Application.Dtos.Responses;

namespace NETCleanArch.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(Guid userId);
    }
}
