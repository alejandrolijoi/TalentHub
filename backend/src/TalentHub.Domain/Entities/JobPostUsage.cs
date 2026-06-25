namespace TalentHub.Domain.Entities;

public class JobPostUsage : BaseEntity
{
    public Guid CompanyId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int JobsPosted { get; set; }

    public Company Company { get; set; } = null!;
}
