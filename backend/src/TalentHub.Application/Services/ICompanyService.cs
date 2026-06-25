using TalentHub.Application.DTOs.Common;
using TalentHub.Application.DTOs.Companies;

namespace TalentHub.Application.Services;

public interface ICompanyService
{
    Task<Result<CompanyResponse>> GetByUserIdAsync(Guid userId);
    Task<Result<CompanyResponse>> UpdateAsync(Guid userId, UpdateCompanyRequest request);
    Task<Result<CompanyProfileResponse>> GetPublicProfileAsync(Guid companyId);
    Task<Result<PaginatedResult<CompanyResponse>>> SearchAsync(string? query, int page, int pageSize);
}
