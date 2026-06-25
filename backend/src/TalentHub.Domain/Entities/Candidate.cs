namespace TalentHub.Domain.Entities;

public class Candidate : BaseEntity
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? Title { get; set; }
    public string? ResumeUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? Website { get; set; }
    public string? Location { get; set; }
    public int? YearsExperience { get; set; }

    public User User { get; set; } = null!;
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
}
