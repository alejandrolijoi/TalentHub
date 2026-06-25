using TalentHub.Application.DTOs.Applications;
using TalentHub.Application.DTOs.Common;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ApplicationResponse>> ApplyAsync(Guid candidateId, ApplyToJobRequest request)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);
        if (job == null || job.Status != JobStatus.Active)
            return Result<ApplicationResponse>.Failure("Job not found or not active");

        var existing = await _unitOfWork.Applications.GetByJobAndCandidateAsync(request.JobId, candidateId);
        if (existing != null)
            return Result<ApplicationResponse>.Failure("You have already applied to this job");

        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            JobId = request.JobId,
            CandidateId = candidateId,
            CoverLetter = request.CoverLetter,
            Status = ApplicationStatus.Applied,
            AppliedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Applications.AddAsync(application);
        job.ApplicationCount++;
        await _unitOfWork.Jobs.UpdateAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return Result<ApplicationResponse>.Success(await MapToResponse(application));
    }

    public async Task<Result> WithdrawAsync(Guid candidateId, Guid applicationId)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
        if (application == null || application.CandidateId != candidateId)
            return Result.Failure("Application not found");

        if (application.Status != ApplicationStatus.Applied)
            return Result.Failure("Cannot withdraw application in current status");

        application.Status = ApplicationStatus.Withdrawn;
        application.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<ApplicationResponse>> UpdateStatusAsync(
        Guid companyId, Guid applicationId, UpdateApplicationStatusRequest request)
    {
        var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
        if (application == null)
            return Result<ApplicationResponse>.Failure("Application not found");

        var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId);
        if (job == null || job.CompanyId != companyId)
            return Result<ApplicationResponse>.Failure("Not authorized");

        application.Status = request.Status;
        application.Notes = request.Notes;
        application.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Applications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();

        return Result<ApplicationResponse>.Success(await MapToResponse(application));
    }

    public async Task<Result<PaginatedResult<ApplicationResponse>>> GetByJobIdAsync(
        Guid companyId, Guid jobId, int page, int pageSize)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null || job.CompanyId != companyId)
            return Result<PaginatedResult<ApplicationResponse>>.Failure("Job not found");

        var applications = await _unitOfWork.Applications.GetByJobIdAsync(jobId);
        var response = new List<ApplicationResponse>();

        foreach (var app in applications.Skip((page - 1) * pageSize).Take(pageSize))
        {
            response.Add(await MapToResponse(app));
        }

        return Result<PaginatedResult<ApplicationResponse>>.Success(new PaginatedResult<ApplicationResponse>
        {
            Items = response,
            TotalCount = applications.Count,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<PaginatedResult<ApplicationResponse>>> GetByCandidateIdAsync(
        Guid candidateId, int page, int pageSize)
    {
        var applications = await _unitOfWork.Applications.GetByCandidateIdAsync(candidateId);
        var response = new List<ApplicationResponse>();

        foreach (var app in applications.Skip((page - 1) * pageSize).Take(pageSize))
        {
            response.Add(await MapToResponse(app));
        }

        return Result<PaginatedResult<ApplicationResponse>>.Success(new PaginatedResult<ApplicationResponse>
        {
            Items = response,
            TotalCount = applications.Count,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<Result<ApplicationStatsResponse>> GetStatsByCandidateIdAsync(Guid candidateId)
    {
        var applications = await _unitOfWork.Applications.GetByCandidateIdAsync(candidateId);

        return Result<ApplicationStatsResponse>.Success(new ApplicationStatsResponse(
            applications.Count,
            applications.Count(a => a.Status == ApplicationStatus.Screening),
            applications.Count(a => a.Status == ApplicationStatus.Interview),
            applications.Count(a => a.Status == ApplicationStatus.Offer),
            applications.Count(a => a.Status == ApplicationStatus.Hired),
            applications.Count(a => a.Status == ApplicationStatus.Rejected)));
    }

    private async Task<ApplicationResponse> MapToResponse(JobApplication application)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId);
        var candidate = await _unitOfWork.Candidates.GetByIdAsync(application.CandidateId);

        return new ApplicationResponse(
            application.Id,
            application.JobId,
            job?.Title ?? "Unknown",
            job?.Company?.Name ?? "Unknown",
            application.CandidateId,
            candidate != null ? $"{candidate.FirstName} {candidate.LastName}" : "Unknown",
            application.Status,
            application.CoverLetter,
            application.AppliedAt);
    }
}
