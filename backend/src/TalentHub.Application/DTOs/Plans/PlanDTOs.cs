namespace TalentHub.Application.DTOs.Plans;

public record PlanResponse(
    Guid Id,
    string Name,
    decimal PriceMonthly,
    decimal PriceYearly,
    string Currency,
    int MaxJobsPerMonth,
    int? MaxApplicantsPerJob,
    Dictionary<string, object> Features);
