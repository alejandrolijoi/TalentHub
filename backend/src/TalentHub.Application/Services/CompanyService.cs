using TalentHub.Application.DTOs.Companies;
using TalentHub.Application.DTOs.Common;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CompanyResponse>> GetByUserIdAsync(Guid userId)
    {
        var company = await _unitOfWork.Companies.GetByUserIdAsync(userId);
        if (company == null)
            return Result<CompanyResponse>.Failure("Company not found");

        return Result<CompanyResponse>.Success(MapToResponse(company));
    }

    public async Task<Result<CompanyResponse>> UpdateAsync(Guid userId, UpdateCompanyRequest request)
    {
        var company = await _unitOfWork.Companies.GetByUserIdAsync(userId);
        if (company == null)
            return Result<CompanyResponse>.Failure("Company not found");

        if (request.Name != null) company.Name = request.Name;
        if (request.Description != null) company.Description = request.Description;
        if (request.LogoUrl != null) company.LogoUrl = request.LogoUrl;
        if (request.Website != null) company.Website = request.Website;
        if (request.LinkedInUrl != null) company.LinkedInUrl = request.LinkedInUrl;
        if (request.Industry != null) company.Industry = request.Industry;
        if (request.CompanySize != null) company.CompanySize = request.CompanySize;
        if (request.Location != null) company.Location = request.Location;
        if (request.FoundedYear.HasValue) company.FoundedYear = request.FoundedYear;

        company.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Companies.UpdateAsync(company);
        await _unitOfWork.SaveChangesAsync();

        return Result<CompanyResponse>.Success(MapToResponse(company));
    }

    public async Task<Result<CompanyProfileResponse>> GetPublicProfileAsync(Guid companyId)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
        if (company == null)
            return Result<CompanyProfileResponse>.Failure("Company not found");

        var jobCount = await _unitOfWork.Jobs.CountAsync();

        return Result<CompanyProfileResponse>.Success(new CompanyProfileResponse(
            company.Id,
            company.Name,
            company.Description,
            company.LogoUrl,
            company.Website,
            company.Industry,
            company.CompanySize,
            company.Location,
            company.FoundedYear,
            jobCount,
            company.CreatedAt));
    }

    public async Task<Result<PaginatedResult<CompanyResponse>>> SearchAsync(string? query, int page, int pageSize)
    {
        var companies = await _unitOfWork.Companies.SearchAsync(query, page, pageSize);
        var totalCount = await _unitOfWork.Companies.CountAsync();
        var response = companies.Select(MapToResponse).ToList();

        return Result<PaginatedResult<CompanyResponse>>.Success(new PaginatedResult<CompanyResponse>
        {
            Items = response,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    private static CompanyResponse MapToResponse(Company company)
    {
        return new CompanyResponse(
            company.Id,
            company.Name,
            company.Description,
            company.LogoUrl,
            company.Website,
            company.LinkedInUrl,
            company.Industry,
            company.CompanySize,
            company.Location,
            company.FoundedYear,
            company.CreatedAt);
    }
}
