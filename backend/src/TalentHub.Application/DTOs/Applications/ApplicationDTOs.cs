using TalentHub.Domain.Enums;

namespace TalentHub.Application.DTOs.Applications;

public record ApplyToJobRequest(Guid JobId, string? CoverLetter);

public record ApplicationResponse(
    Guid Id,
    Guid JobId,
    string JobTitle,
    string CompanyName,
    Guid CandidateId,
    string CandidateName,
    ApplicationStatus Status,
    string? CoverLetter,
    DateTime AppliedAt);

public record UpdateApplicationStatusRequest(
    ApplicationStatus Status,
    string? Notes);

public record ApplicationStatsResponse(
    int TotalApplied,
    int Screening,
    int Interview,
    int Offer,
    int Hired,
    int Rejected);
