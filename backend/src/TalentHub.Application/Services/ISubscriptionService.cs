using TalentHub.Application.DTOs.Common;
using TalentHub.Application.DTOs.Plans;
using TalentHub.Application.DTOs.Subscriptions;

namespace TalentHub.Application.Services;

public interface ISubscriptionService
{
    Task<Result<IReadOnlyList<PlanResponse>>> GetPlansAsync();
    Task<Result<PlanResponse>> GetPlanByIdAsync(Guid planId);
    Task<Result<SubscriptionResponse>> GetCurrentSubscriptionAsync(Guid companyId);
    Task<Result<CheckoutResponse>> CreateCheckoutSessionAsync(Guid companyId, CheckoutRequest request);
    Task<Result> ChangePlanAsync(Guid companyId, ChangePlanRequest request);
    Task<Result> CancelSubscriptionAsync(Guid companyId);
    Task<Result> HandleStripeWebhookAsync(string payload, string signature);
    Task<Result<BillingPortalResponse>> GetBillingPortalUrlAsync(Guid companyId);
    Task<Result<PaginatedResult<InvoiceResponse>>> GetInvoicesAsync(Guid companyId, int page, int pageSize);
    Task<Result<int>> GetCurrentMonthUsageAsync(Guid companyId);
}
