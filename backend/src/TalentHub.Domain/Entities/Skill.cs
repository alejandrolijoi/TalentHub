namespace TalentHub.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }

    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
}
