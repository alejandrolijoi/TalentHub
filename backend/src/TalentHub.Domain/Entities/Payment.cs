using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid? SubscriptionId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentProvider PaymentProvider { get; set; }
    public string? ProviderPaymentId { get; set; }
    public PaymentStatus Status { get; set; }
    public string? InvoiceUrl { get; set; }
    public string? Description { get; set; }

    public Subscription? Subscription { get; set; }
    public Company Company { get; set; } = null!;
}
