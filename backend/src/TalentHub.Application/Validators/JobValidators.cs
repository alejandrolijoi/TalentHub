using FluentValidation;
using TalentHub.Application.DTOs.Jobs;

namespace TalentHub.Application.Validators;

public class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
{
    public CreateJobRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(50).WithMessage("Description must be at least 50 characters");

        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0).When(x => x.SalaryMin.HasValue)
            .WithMessage("Minimum salary must be non-negative");

        RuleFor(x => x.SalaryMax)
            .GreaterThanOrEqualTo(x => x.SalaryMin ?? 0).When(x => x.SalaryMax.HasValue)
            .WithMessage("Maximum salary must be greater than or equal to minimum salary");
    }
}
