using TalentHub.Application.DTOs.Common;
using TalentHub.Application.DTOs.Jobs;

namespace TalentHub.Application.Services;

public interface IJobService
{
    Task<Result<JobResponse>> GetByIdAsync(Guid id);
    Task<Result<JobResponse>> CreateAsync(Guid companyId, CreateJobRequest request);
    Task<Result<JobResponse>> UpdateAsync(Guid companyId, Guid jobId, UpdateJobRequest request);
    Task<Result> DeleteAsync(Guid companyId, Guid jobId);
    Task<Result<PaginatedResult<JobResponse>>> SearchAsync(JobSearchRequest request);
    Task<Result<IReadOnlyList<JobResponse>>> GetFeaturedJobsAsync(int limit = 10);
    Task<Result<PaginatedResult<JobResponse>>> GetByCompanyIdAsync(Guid companyId, int page, int pageSize);
    Task<Result<IReadOnlyList<JobResponse>>> GetByCandidateIdAsync(Guid candidateId);
    Task<Result> IncrementViewCountAsync(Guid jobId);
}
