using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Entities;

public class JobApplication : BaseEntity
{
    public Guid JobId { get; set; }
    public Guid CandidateId { get; set; }
    public string? CoverLetter { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? Notes { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public Job Job { get; set; } = null!;
    public Candidate Candidate { get; set; } = null!;
}
