using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Entities;

public class Job : BaseEntity
{
    public Guid CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Benefits { get; set; }
    public Guid? CategoryId { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public string? Location { get; set; }
    public RemoteType RemoteType { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string Currency { get; set; } = "USD";
    public JobStatus Status { get; set; } = JobStatus.Draft;
    public bool IsFeatured { get; set; }
    public DateTime? FeaturedUntil { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }

    public Company Company { get; set; } = null!;
    public Category? Category { get; set; }
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
}
