using TalentHub.Application.DTOs.Applications;
using TalentHub.Application.DTOs.Common;

namespace TalentHub.Application.Services;

public interface IApplicationService
{
    Task<Result<ApplicationResponse>> ApplyAsync(Guid candidateId, ApplyToJobRequest request);
    Task<Result> WithdrawAsync(Guid candidateId, Guid applicationId);
    Task<Result<ApplicationResponse>> UpdateStatusAsync(Guid companyId, Guid applicationId, UpdateApplicationStatusRequest request);
    Task<Result<PaginatedResult<ApplicationResponse>>> GetByJobIdAsync(Guid companyId, Guid jobId, int page, int pageSize);
    Task<Result<PaginatedResult<ApplicationResponse>>> GetByCandidateIdAsync(Guid candidateId, int page, int pageSize);
    Task<Result<ApplicationStatsResponse>> GetStatsByCandidateIdAsync(Guid candidateId);
}
