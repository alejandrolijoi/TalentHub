using TalentHub.Application.DTOs.Common;

namespace TalentHub.Application.Services;

public interface ISkillService
{
    Task<Result<IReadOnlyList<SkillResponse>>> GetAllAsync();
    Task<Result<IReadOnlyList<SkillResponse>>> SearchAsync(string query);
}

public record SkillResponse(Guid Id, string Name, string? Category);
