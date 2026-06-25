namespace TalentHub.Application.DTOs.Candidates;

public record CandidateResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Phone,
    string? Bio,
    string? Title,
    string? ResumeUrl,
    string? LinkedInUrl,
    string? GithubUrl,
    string? Website,
    string? Location,
    int? YearsExperience,
    List<string> Skills,
    DateTime CreatedAt);

public record UpdateCandidateRequest(
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Bio,
    string? Title,
    string? LinkedInUrl,
    string? GithubUrl,
    string? Website,
    string? Location,
    int? YearsExperience);

public record UpdateCandidateSkillsRequest(
    List<SkillProficiencyRequest> Skills);

public record SkillProficiencyRequest(
    Guid SkillId,
    int ProficiencyLevel);
