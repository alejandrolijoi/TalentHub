using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid CompanyId { get; set; }
    public Guid PlanId { get; set; }
    public PaymentProvider PaymentProvider { get; set; }
    public string? ProviderSubscriptionId { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    public DateTime? TrialEnd { get; set; }

    public Company Company { get; set; } = null!;
    public Plan Plan { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
