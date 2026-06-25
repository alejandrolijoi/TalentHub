using TalentHub.Domain.Enums;

namespace TalentHub.Application.DTOs.Jobs;

public record CreateJobRequest(
    string Title,
    string Description,
    string? Requirements,
    string? Benefits,
    Guid? CategoryId,
    EmploymentType EmploymentType,
    ExperienceLevel ExperienceLevel,
    string? Location,
    RemoteType RemoteType,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string Currency,
    List<Guid>? SkillIds);

public record UpdateJobRequest(
    string? Title,
    string? Description,
    string? Requirements,
    string? Benefits,
    Guid? CategoryId,
    EmploymentType? EmploymentType,
    ExperienceLevel? ExperienceLevel,
    string? Location,
    RemoteType? RemoteType,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string? Currency,
    JobStatus? Status,
    List<Guid>? SkillIds);

public record JobResponse(
    Guid Id,
    string Title,
    string Description,
    string? Requirements,
    string? Benefits,
    Guid CompanyId,
    string CompanyName,
    string? CompanyLogoUrl,
    Guid? CategoryId,
    string? CategoryName,
    EmploymentType EmploymentType,
    ExperienceLevel ExperienceLevel,
    string? Location,
    RemoteType RemoteType,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string Currency,
    JobStatus Status,
    bool IsFeatured,
    int ViewCount,
    int ApplicationCount,
    List<string> Skills,
    DateTime CreatedAt);

public record JobSearchRequest(
    string? Query,
    Guid? CategoryId,
    EmploymentType? EmploymentType,
    ExperienceLevel? ExperienceLevel,
    RemoteType? RemoteType,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string? Location,
    int Page = 1,
    int PageSize = 20);
