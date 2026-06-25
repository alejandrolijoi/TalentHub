using TalentHub.Application.DTOs.Common;
using TalentHub.Application.DTOs.Jobs;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class JobService : IJobService
{
    private readonly IUnitOfWork _unitOfWork;

    public JobService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<JobResponse>> GetByIdAsync(Guid id)
    {
        var job = await _unitOfWork.Jobs.GetWithDetailsAsync(id);
        if (job == null)
            return Result<JobResponse>.Failure("Job not found");

        return Result<JobResponse>.Success(MapToResponse(job));
    }

    public async Task<Result<JobResponse>> CreateAsync(Guid companyId, CreateJobRequest request)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            Benefits = request.Benefits,
            CategoryId = request.CategoryId,
            EmploymentType = request.EmploymentType,
            ExperienceLevel = request.ExperienceLevel,
            Location = request.Location,
            RemoteType = request.RemoteType,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            Currency = request.Currency,
            Status = JobStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (request.SkillIds?.Any() == true)
        {
            foreach (var skillId in request.SkillIds)
            {
                job.JobSkills.Add(new JobSkill { JobId = job.Id, SkillId = skillId });
            }
        }

        await _unitOfWork.Jobs.AddAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return Result<JobResponse>.Success(MapToResponse(job));
    }

    public async Task<Result<JobResponse>> UpdateAsync(Guid companyId, Guid jobId, UpdateJobRequest request)
    {
        var job = await _unitOfWork.Jobs.GetWithDetailsAsync(jobId);
        if (job == null || job.CompanyId != companyId)
            return Result<JobResponse>.Failure("Job not found");

        if (request.Title != null) job.Title = request.Title;
        if (request.Description != null) job.Description = request.Description;
        if (request.Requirements != null) job.Requirements = request.Requirements;
        if (request.Benefits != null) job.Benefits = request.Benefits;
        if (request.CategoryId.HasValue) job.CategoryId = request.CategoryId;
        if (request.EmploymentType.HasValue) job.EmploymentType = request.EmploymentType.Value;
        if (request.ExperienceLevel.HasValue) job.ExperienceLevel = request.ExperienceLevel.Value;
        if (request.Location != null) job.Location = request.Location;
        if (request.RemoteType.HasValue) job.RemoteType = request.RemoteType.Value;
        if (request.SalaryMin.HasValue) job.SalaryMin = request.SalaryMin;
        if (request.SalaryMax.HasValue) job.SalaryMax = request.SalaryMax;
        if (request.Currency != null) job.Currency = request.Currency;
        if (request.Status.HasValue) job.Status = request.Status.Value;

        job.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Jobs.UpdateAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return Result<JobResponse>.Success(MapToResponse(job));
    }

    public async Task<Result> DeleteAsync(Guid companyId, Guid jobId)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null || job.CompanyId != companyId)
            return Result.Failure("Job not found");

        await _unitOfWork.Jobs.DeleteAsync(job);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<PaginatedResult<JobResponse>>> SearchAsync(JobSearchRequest request)
    {
        var jobs = await _unitOfWork.Jobs.SearchAsync(
            request.Query, request.CategoryId, request.EmploymentType,
            request.ExperienceLevel, request.RemoteType, request.SalaryMin,
            request.SalaryMax, request.Location, request.Page, request.PageSize);

        var totalCount = await _unitOfWork.Jobs.CountAsync();
        var response = jobs.Select(MapToResponse).ToList();

        return Result<PaginatedResult<JobResponse>>.Success(new PaginatedResult<JobResponse>
        {
            Items = response,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }

    public async Task<Result<IReadOnlyList<JobResponse>>> GetFeaturedJobsAsync(int limit = 10)
    {
        var jobs = await _unitOfWork.Jobs.GetFeaturedJobsAsync(limit);
        return Result<IReadOnlyList<JobResponse>>.Success(jobs.Select(MapToResponse).ToList());
    }

    public async Task<Result<PaginatedResult<JobResponse>>> GetByCompanyIdAsync(Guid companyId, int page, int pageSize)
    {
        var jobs = await _unitOfWork.Jobs.GetByCompanyIdAsync(companyId, page, pageSize);
        var totalCount = await _unitOfWork.Jobs.CountAsync();
        var response = jobs.Select(MapToResponse).ToList();

        return Result<PaginatedResult<JobResponse>>.Success(new PaginatedResult<JobResponse>
        {
            Items = response,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<IReadOnlyList<JobResponse>>> GetByCandidateIdAsync(Guid candidateId)
    {
        var jobs = await _unitOfWork.Jobs.GetByCandidateIdAsync(candidateId);
        return Result<IReadOnlyList<JobResponse>>.Success(jobs.Select(MapToResponse).ToList());
    }

    public async Task<Result> IncrementViewCountAsync(Guid jobId)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null) return Result.Failure("Job not found");

        job.ViewCount++;
        await _unitOfWork.Jobs.UpdateAsync(job);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private static JobResponse MapToResponse(Job job)
    {
        return new JobResponse(
            job.Id,
            job.Title,
            job.Description,
            job.Requirements,
            job.Benefits,
            job.CompanyId,
            job.Company?.Name ?? "Unknown",
            job.Company?.LogoUrl,
            job.CategoryId,
            job.Category?.Name,
            job.EmploymentType,
            job.ExperienceLevel,
            job.Location,
            job.RemoteType,
            job.SalaryMin,
            job.SalaryMax,
            job.Currency,
            job.Status,
            job.IsFeatured,
            job.ViewCount,
            job.ApplicationCount,
            job.JobSkills?.Select(js => js.Skill?.Name ?? "").ToList() ?? new List<string>(),
            job.CreatedAt);
    }
}
