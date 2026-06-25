using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TalentHub.Application.DTOs.Auth;
using TalentHub.Application.DTOs.Common;
using TalentHub.Application.Validators;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        var validator = new RegisterRequestValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return Result<AuthResponse>.Failure(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

        var email = request.Email.ToLower().Trim();
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
        if (existingUser != null)
            return Result<AuthResponse>.Failure("Email already registered");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);

        if (request.Role == UserRole.Candidate)
        {
            var candidate = new Candidate
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Candidates.AddAsync(candidate);
        }
        else if (request.Role == UserRole.Company)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = $"{request.FirstName} {request.LastName}'s Company",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Companies.AddAsync(company);
        }

        await _unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, user.Email, user.Role, token, refreshToken));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var validator = new LoginRequestValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return Result<AuthResponse>.Failure(string.Join(", ", validation.Errors.Select(e => e.ErrorMessage)));

        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.ToLower().Trim());
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Invalid email or password");

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return Result<AuthResponse>.Success(new AuthResponse(
            user.Id, user.Email, user.Role, token, refreshToken));
    }

    public Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        return Task.FromResult(Result<AuthResponse>.Failure("Refresh token functionality not implemented"));
    }

    public Task<Result> ForgotPasswordAsync(string email)
    {
        return Task.FromResult(Result.Success());
    }

    public Task<Result> ResetPasswordAsync(string token, string newPassword)
    {
        return Task.FromResult(Result.Success());
    }

    public Task<Result> RevokeRefreshTokenAsync(string refreshToken)
    {
        return Task.FromResult(Result.Success());
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT secret key not configured")));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "15")),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
