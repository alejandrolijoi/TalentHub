using TalentHub.Application.DTOs.Candidates;
using TalentHub.Application.DTOs.Common;

namespace TalentHub.Application.Services;

public interface ICandidateService
{
    Task<Result<CandidateResponse>> GetByUserIdAsync(Guid userId);
    Task<Result<CandidateResponse>> GetPublicProfileAsync(Guid candidateId);
    Task<Result<CandidateResponse>> UpdateAsync(Guid userId, UpdateCandidateRequest request);
    Task<Result<CandidateResponse>> UpdateSkillsAsync(Guid userId, UpdateCandidateSkillsRequest request);
    Task<Result<string>> UploadResumeAsync(Guid userId, Stream fileStream, string fileName);
}
