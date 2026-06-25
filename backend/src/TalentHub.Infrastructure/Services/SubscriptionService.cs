using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using TalentHub.Application.DTOs.Common;
using TalentHub.Application.DTOs.Plans;
using TalentHub.Application.DTOs.Subscriptions;
using TalentHub.Application.Services;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;
using TalentHub.Domain.Interfaces;

namespace TalentHub.Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public SubscriptionService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
    }

    public async Task<Result<IReadOnlyList<PlanResponse>>> GetPlansAsync()
    {
        var plans = await _unitOfWork.Plans.GetAllAsync();
        var activePlans = plans.Where(p => p.IsActive).ToList();

        var response = activePlans.Select(p => new PlanResponse(
            p.Id, p.Name, p.PriceMonthly, p.PriceYearly, p.Currency,
            p.MaxJobsPerMonth, p.MaxApplicantsPerJob, p.Features)).ToList();

        return Result<IReadOnlyList<PlanResponse>>.Success(response);
    }

    public async Task<Result<PlanResponse>> GetPlanByIdAsync(Guid planId)
    {
        var plan = await _unitOfWork.Plans.GetByIdAsync(planId);
        if (plan == null)
            return Result<PlanResponse>.Failure("Plan not found");

        return Result<PlanResponse>.Success(new PlanResponse(
            plan.Id, plan.Name, plan.PriceMonthly, plan.PriceYearly, plan.Currency,
            plan.MaxJobsPerMonth, plan.MaxApplicantsPerJob, plan.Features));
    }

    public async Task<Result<SubscriptionResponse>> GetCurrentSubscriptionAsync(Guid companyId)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByCompanyIdAsync(companyId);
        if (subscription == null)
            return Result<SubscriptionResponse>.Failure("No active subscription");

        return Result<SubscriptionResponse>.Success(new SubscriptionResponse(
            subscription.Id, subscription.PlanId, subscription.Plan?.Name ?? "Unknown",
            subscription.PaymentProvider, subscription.Status,
            subscription.CurrentPeriodStart, subscription.CurrentPeriodEnd,
            subscription.CancelAtPeriodEnd));
    }

    public async Task<Result<CheckoutResponse>> CreateCheckoutSessionAsync(Guid companyId, CheckoutRequest request)
    {
        var plan = await _unitOfWork.Plans.GetByIdAsync(request.PlanId);
        if (plan == null)
            return Result<CheckoutResponse>.Failure("Plan not found");

        var existingSubscription = await _unitOfWork.Subscriptions.GetByCompanyIdAsync(companyId);
        if (existingSubscription != null && existingSubscription.Status == SubscriptionStatus.Active)
            return Result<CheckoutResponse>.Failure("You already have an active subscription");

        if (request.PaymentProvider == PaymentProvider.Stripe)
            return await CreateStripeCheckoutSession(companyId, plan);

        return Result<CheckoutResponse>.Failure("Invalid payment provider");
    }

    private async Task<Result<CheckoutResponse>> CreateStripeCheckoutSession(Guid companyId, Domain.Entities.Plan plan)
    {
        var priceId = plan.StripePriceId;
        if (string.IsNullOrEmpty(priceId))
            return Result<CheckoutResponse>.Failure("Stripe price not configured for this plan");

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new() { Price = priceId, Quantity = 1 },
            },
            Mode = "subscription",
            SuccessUrl = $"{_configuration["Frontend:Url"]}/dashboard/company/billing?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_configuration["Frontend:Url"]}/pricing?canceled=true",
            Metadata = new Dictionary<string, string>
            {
                { "companyId", companyId.ToString() },
                { "planId", plan.Id.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return Result<CheckoutResponse>.Success(new CheckoutResponse(session.Id, session.Url!));
    }

    public async Task<Result> ChangePlanAsync(Guid companyId, ChangePlanRequest request)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByCompanyIdAsync(companyId);
        if (subscription == null)
            return Result.Failure("No active subscription");

        var newPlan = await _unitOfWork.Plans.GetByIdAsync(request.NewPlanId);
        if (newPlan == null)
            return Result.Failure("New plan not found");

        if (subscription.PaymentProvider == PaymentProvider.Stripe && !string.IsNullOrEmpty(subscription.ProviderSubscriptionId))
        {
            var stripeService = new Stripe.SubscriptionService();
            var updateOptions = new SubscriptionUpdateOptions
            {
                Items = new List<SubscriptionItemOptions>
                {
                    new() { Price = newPlan.StripePriceId },
                },
                ProrationBehavior = "create_prorations",
            };
            await stripeService.UpdateAsync(subscription.ProviderSubscriptionId, updateOptions);
        }

        subscription.PlanId = request.NewPlanId;
        subscription.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Subscriptions.UpdateAsync(subscription);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> CancelSubscriptionAsync(Guid companyId)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByCompanyIdAsync(companyId);
        if (subscription == null)
            return Result.Failure("No active subscription");

        if (subscription.PaymentProvider == PaymentProvider.Stripe && !string.IsNullOrEmpty(subscription.ProviderSubscriptionId))
        {
            var stripeService = new Stripe.SubscriptionService();
            var updateOptions = new SubscriptionUpdateOptions { CancelAtPeriodEnd = true };
            await stripeService.UpdateAsync(subscription.ProviderSubscriptionId, updateOptions);
        }

        subscription.CancelAtPeriodEnd = true;
        subscription.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Subscriptions.UpdateAsync(subscription);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> HandleStripeWebhookAsync(string payload, string signature)
    {
        var webhookSecret = _configuration["Stripe:WebhookSecret"];
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(payload, signature, webhookSecret!);
        }
        catch
        {
            return Result.Failure("Invalid webhook signature");
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                await HandleCheckoutCompleted(stripeEvent.Data.Object as Session);
                break;
            case "customer.subscription.updated":
                await HandleSubscriptionUpdated(stripeEvent.Data.Object as Stripe.Subscription);
                break;
            case "customer.subscription.deleted":
                await HandleSubscriptionDeleted(stripeEvent.Data.Object as Stripe.Subscription);
                break;
        }

        return Result.Success();
    }

    private async Task HandleCheckoutCompleted(Session? session)
    {
        if (session?.Metadata == null) return;

        var companyId = Guid.Parse(session.Metadata["companyId"]);
        var planId = Guid.Parse(session.Metadata["planId"]);

        var subscription = new Domain.Entities.Subscription
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            PlanId = planId,
            PaymentProvider = PaymentProvider.Stripe,
            ProviderSubscriptionId = session.SubscriptionId,
            Status = SubscriptionStatus.Active,
            CurrentPeriodStart = DateTime.UtcNow,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Subscriptions.AddAsync(subscription);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            SubscriptionId = subscription.Id,
            CompanyId = companyId,
            Amount = (session.AmountTotal ?? 0) / 100m,
            Currency = session.Currency?.ToUpper() ?? "USD",
            PaymentProvider = PaymentProvider.Stripe,
            ProviderPaymentId = session.PaymentIntentId,
            Status = PaymentStatus.Succeeded,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task HandleSubscriptionUpdated(Stripe.Subscription? stripeSubscription)
    {
        if (stripeSubscription?.Id == null) return;

        var subscription = await _unitOfWork.Subscriptions.GetByProviderSubscriptionIdAsync(stripeSubscription.Id);
        if (subscription == null) return;

        subscription.Status = stripeSubscription.Status == "active"
            ? SubscriptionStatus.Active
            : stripeSubscription.Status == "past_due"
                ? SubscriptionStatus.PastDue
                : SubscriptionStatus.Canceled;

        subscription.CurrentPeriodStart = stripeSubscription.CurrentPeriodStart;
        subscription.CurrentPeriodEnd = stripeSubscription.CurrentPeriodEnd;
        subscription.CancelAtPeriodEnd = stripeSubscription.CancelAtPeriodEnd;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Subscriptions.UpdateAsync(subscription);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task HandleSubscriptionDeleted(Stripe.Subscription? stripeSubscription)
    {
        if (stripeSubscription?.Id == null) return;

        var subscription = await _unitOfWork.Subscriptions.GetByProviderSubscriptionIdAsync(stripeSubscription.Id);
        if (subscription == null) return;

        subscription.Status = SubscriptionStatus.Canceled;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Subscriptions.UpdateAsync(subscription);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Result<BillingPortalResponse>> GetBillingPortalUrlAsync(Guid companyId)
    {
        var subscription = await _unitOfWork.Subscriptions.GetByCompanyIdAsync(companyId);
        if (subscription == null || string.IsNullOrEmpty(subscription.ProviderSubscriptionId))
            return Result<BillingPortalResponse>.Failure("No active subscription");

        if (subscription.PaymentProvider == PaymentProvider.Stripe)
        {
            var service = new Stripe.BillingPortal.SessionService();
            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = subscription.ProviderSubscriptionId,
                ReturnUrl = $"{_configuration["Frontend:Url"]}/dashboard/company/billing",
            };
            var session = await service.CreateAsync(options);
            return Result<BillingPortalResponse>.Success(new BillingPortalResponse(session.Url));
        }

        return Result<BillingPortalResponse>.Failure("Billing portal not available");
    }

    public async Task<Result<PaginatedResult<InvoiceResponse>>> GetInvoicesAsync(Guid companyId, int page, int pageSize)
    {
        var payments = await _unitOfWork.Payments.GetByCompanyIdAsync(companyId);
        var response = payments
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new InvoiceResponse(p.Id, p.Amount, p.Currency, p.Status, p.InvoiceUrl, p.CreatedAt))
            .ToList();

        return Result<PaginatedResult<InvoiceResponse>>.Success(new PaginatedResult<InvoiceResponse>
        {
            Items = response, TotalCount = payments.Count, Page = page, PageSize = pageSize
        });
    }

    public async Task<Result<int>> GetCurrentMonthUsageAsync(Guid companyId)
    {
        var now = DateTime.UtcNow;
        var usage = await _unitOfWork.JobPostUsages.GetByCompanyAndMonthAsync(companyId, now.Year, now.Month);
        return Result<int>.Success(usage?.JobsPosted ?? 0);
    }
}
