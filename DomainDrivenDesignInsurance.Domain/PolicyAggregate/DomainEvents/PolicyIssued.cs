namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.DomainEvents;

public sealed class PolicyIssued : DomainEvent
{
    public Guid PolicyId { get; }
    public Guid InsuredId { get; }
    public Guid BrokerId { get; }


    public PolicyIssued(Guid policyId, Guid insuredId, Guid brokerId)
    {
        PolicyId = policyId; InsuredId = insuredId; BrokerId = brokerId;
    }
}