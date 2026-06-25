namespace TalentHub.Application.DTOs.Companies;

public record CompanyResponse(
    Guid Id,
    string Name,
    string? Description,
    string? LogoUrl,
    string? Website,
    string? LinkedInUrl,
    string? Industry,
    string? CompanySize,
    string? Location,
    int? FoundedYear,
    DateTime CreatedAt);

public record UpdateCompanyRequest(
    string? Name,
    string? Description,
    string? LogoUrl,
    string? Website,
    string? LinkedInUrl,
    string? Industry,
    string? CompanySize,
    string? Location,
    int? FoundedYear);

public record CompanyProfileResponse(
    Guid Id,
    string Name,
    string? Description,
    string? LogoUrl,
    string? Website,
    string? Industry,
    string? CompanySize,
    string? Location,
    int? FoundedYear,
    int TotalJobs,
    DateTime CreatedAt);
