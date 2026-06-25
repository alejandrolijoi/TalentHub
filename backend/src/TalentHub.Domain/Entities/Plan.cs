using System.Text.Json;

namespace TalentHub.Domain.Entities;

public class Plan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? StripePriceId { get; set; }
    public string? MercadoPagoPriceId { get; set; }
    public decimal PriceMonthly { get; set; }
    public decimal PriceYearly { get; set; }
    public string Currency { get; set; } = "USD";
    public int MaxJobsPerMonth { get; set; }
    public int? MaxApplicantsPerJob { get; set; }
    public string FeaturesJson { get; set; } = "{}";
    public bool IsActive { get; set; } = true;

    public Dictionary<string, object> Features =>
        JsonSerializer.Deserialize<Dictionary<string, object>>(FeaturesJson) ?? new();

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
