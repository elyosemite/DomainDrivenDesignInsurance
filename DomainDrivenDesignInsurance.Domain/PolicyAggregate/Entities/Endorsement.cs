using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

/// <summary>
/// Endorsement is modelled as an entity within the Policy aggregate.
/// </summary>
public class Endorsement
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public Money PremiumDelta { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Endorsement(string type, DateTime effectiveDate, Money premiumDelta)
    {
        Id = Guid.NewGuid();
        Type = type ?? throw new ArgumentNullException(nameof(type));
        EffectiveDate = effectiveDate;
        PremiumDelta = premiumDelta ?? throw new ArgumentNullException(nameof(premiumDelta));
        CreatedAt = DateTime.UtcNow;
    }
}