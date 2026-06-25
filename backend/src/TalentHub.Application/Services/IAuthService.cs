using TalentHub.Application.DTOs.Auth;
using TalentHub.Application.DTOs.Common;

namespace TalentHub.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    Task<Result> ForgotPasswordAsync(string email);
    Task<Result> ResetPasswordAsync(string token, string newPassword);
    Task<Result> RevokeRefreshTokenAsync(string refreshToken);
}
