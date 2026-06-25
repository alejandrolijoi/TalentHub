using TalentHub.Application.DTOs.Common;

namespace TalentHub.Application.Services;

public interface ICategoryService
{
    Task<Result<IReadOnlyList<CategoryResponse>>> GetAllAsync();
    Task<Result<CategoryResponse>> GetBySlugAsync(string slug);
}

public record CategoryResponse(Guid Id, string Name, string Slug, string? Icon, int JobCount);
