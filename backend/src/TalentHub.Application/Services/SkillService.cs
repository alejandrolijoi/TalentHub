using TalentHub.Application.DTOs.Common;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;

    public SkillService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<SkillResponse>>> GetAllAsync()
    {
        var skills = await _unitOfWork.Skills.GetAllAsync();
        var response = skills.Select(s => new SkillResponse(s.Id, s.Name, s.Category)).ToList();
        return Result<IReadOnlyList<SkillResponse>>.Success(response);
    }

    public async Task<Result<IReadOnlyList<SkillResponse>>> SearchAsync(string query)
    {
        var skills = await _unitOfWork.Skills.SearchAsync(query);
        var response = skills.Select(s => new SkillResponse(s.Id, s.Name, s.Category)).ToList();
        return Result<IReadOnlyList<SkillResponse>>.Success(response);
    }
}
