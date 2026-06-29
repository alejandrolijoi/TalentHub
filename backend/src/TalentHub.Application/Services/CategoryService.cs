using TalentHub.Application.DTOs.Common;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<CategoryResponse>>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetWithJobCountsAsync();
        var response = categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Slug, c.Icon, c.JobCount)).ToList();
        return Result<IReadOnlyList<CategoryResponse>>.Success(response);
    }

    public async Task<Result<CategoryResponse>> GetBySlugAsync(string slug)
    {
        var category = await _unitOfWork.Categories.GetBySlugAsync(slug);
        if (category == null)
            return Result<CategoryResponse>.Failure("Category not found");

        return Result<CategoryResponse>.Success(new CategoryResponse(category.Id, category.Name, category.Slug, category.Icon, category.JobCount));
    }
}
