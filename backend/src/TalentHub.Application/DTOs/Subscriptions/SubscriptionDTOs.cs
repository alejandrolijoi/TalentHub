using TalentHub.Domain.Enums;

namespace TalentHub.Application.DTOs.Subscriptions;

public record SubscriptionResponse(
    Guid Id,
    Guid PlanId,
    string PlanName,
    PaymentProvider PaymentProvider,
    SubscriptionStatus Status,
    DateTime CurrentPeriodStart,
    DateTime CurrentPeriodEnd,
    bool CancelAtPeriodEnd);

public record CheckoutRequest(
    Guid PlanId,
    PaymentProvider PaymentProvider);

public record CheckoutResponse(
    string SessionId,
    string Url);

public record ChangePlanRequest(Guid NewPlanId);

public record BillingPortalResponse(string Url);

public record InvoiceResponse(
    Guid Id,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    string? InvoiceUrl,
    DateTime CreatedAt);
