using TalentHub.Application.DTOs.Candidates;
using TalentHub.Application.DTOs.Common;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class CandidateService : ICandidateService
{
    private readonly IUnitOfWork _unitOfWork;

    public CandidateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CandidateResponse>> GetByUserIdAsync(Guid userId)
    {
        var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
        if (candidate == null)
            return Result<CandidateResponse>.Failure("Candidate not found");

        return Result<CandidateResponse>.Success(MapToResponse(candidate));
    }

    public async Task<Result<CandidateResponse>> GetPublicProfileAsync(Guid candidateId)
    {
        var candidate = await _unitOfWork.Candidates.GetWithDetailsAsync(candidateId);
        if (candidate == null)
            return Result<CandidateResponse>.Failure("Candidate not found");

        return Result<CandidateResponse>.Success(MapToResponse(candidate));
    }

    public async Task<Result<CandidateResponse>> UpdateAsync(Guid userId, UpdateCandidateRequest request)
    {
        var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
        if (candidate == null)
            return Result<CandidateResponse>.Failure("Candidate not found");

        if (request.FirstName != null) candidate.FirstName = request.FirstName;
        if (request.LastName != null) candidate.LastName = request.LastName;
        if (request.Phone != null) candidate.Phone = request.Phone;
        if (request.Bio != null) candidate.Bio = request.Bio;
        if (request.Title != null) candidate.Title = request.Title;
        if (request.LinkedInUrl != null) candidate.LinkedInUrl = request.LinkedInUrl;
        if (request.GithubUrl != null) candidate.GithubUrl = request.GithubUrl;
        if (request.Website != null) candidate.Website = request.Website;
        if (request.Location != null) candidate.Location = request.Location;
        if (request.YearsExperience.HasValue) candidate.YearsExperience = request.YearsExperience;

        candidate.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Candidates.UpdateAsync(candidate);
        await _unitOfWork.SaveChangesAsync();

        return Result<CandidateResponse>.Success(MapToResponse(candidate));
    }

    public async Task<Result<CandidateResponse>> UpdateSkillsAsync(Guid userId, UpdateCandidateSkillsRequest request)
    {
        var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
        if (candidate == null)
            return Result<CandidateResponse>.Failure("Candidate not found");

        candidate.CandidateSkills.Clear();
        foreach (var skill in request.Skills)
        {
            candidate.CandidateSkills.Add(new CandidateSkill
            {
                CandidateId = candidate.Id,
                SkillId = skill.SkillId,
                ProficiencyLevel = skill.ProficiencyLevel
            });
        }

        candidate.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Candidates.UpdateAsync(candidate);
        await _unitOfWork.SaveChangesAsync();

        return Result<CandidateResponse>.Success(MapToResponse(candidate));
    }

    public Task<Result<string>> UploadResumeAsync(Guid userId, Stream fileStream, string fileName)
    {
        return Task.FromResult(Result<string>.Success($"/resumes/{userId}/{fileName}"));
    }

    private static CandidateResponse MapToResponse(Candidate candidate)
    {
        return new CandidateResponse(
            candidate.Id,
            candidate.FirstName,
            candidate.LastName,
            candidate.Phone,
            candidate.Bio,
            candidate.Title,
            candidate.ResumeUrl,
            candidate.LinkedInUrl,
            candidate.GithubUrl,
            candidate.Website,
            candidate.Location,
            candidate.YearsExperience,
            candidate.CandidateSkills?.Select(cs => cs.Skill?.Name ?? "").ToList() ?? new List<string>(),
            candidate.CreatedAt);
    }
}
