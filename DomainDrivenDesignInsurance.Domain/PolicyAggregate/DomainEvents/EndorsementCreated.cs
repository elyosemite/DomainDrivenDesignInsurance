namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.DomainEvents;

public sealed class EndorsementCreated : DomainEvent
{
    public Guid PolicyId { get; }
    public Guid EndorsementId { get; }
    public Guid InsuredId { get; }
    public string EndorsementType { get; }
    public decimal PremiumDelta { get; }


    public EndorsementCreated(Guid policyId, Guid endorsementId, Guid insuredId, string endorsementType, decimal premiumDelta)
    {
        PolicyId = policyId; EndorsementId = endorsementId; InsuredId = insuredId; EndorsementType = endorsementType; PremiumDelta = premiumDelta;
    }
}