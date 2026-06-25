using TalentHub.Domain.Enums;

namespace TalentHub.Application.DTOs.Auth;

public record RegisterRequest(
    string Email,
    string Password,
    UserRole Role,
    string FirstName,
    string LastName);

public record LoginRequest(string Email, string Password);

public record AuthResponse(
    Guid UserId,
    string Email,
    UserRole Role,
    string Token,
    string RefreshToken);

public record RefreshTokenRequest(string RefreshToken);

public record ForgotPasswordRequest(string Email);

public record ResetPasswordRequest(string Token, string NewPassword);
