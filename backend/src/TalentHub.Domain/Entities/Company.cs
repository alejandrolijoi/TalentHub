namespace TalentHub.Domain.Entities;

public class Company : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? Industry { get; set; }
    public string? CompanySize { get; set; }
    public string? Location { get; set; }
    public int? FoundedYear { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public Subscription? Subscription { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
