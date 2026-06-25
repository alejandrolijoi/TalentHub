using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? AvatarUrl { get; set; }

    public Candidate? Candidate { get; set; }
    public Company? Company { get; set; }
}
