namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.DomainEvents;

public sealed class PolicyCancelled : DomainEvent
{
    public Guid PolicyId { get; }
    public string Reason { get; }


    public PolicyCancelled(Guid policyId, string reason)
    {
        PolicyId = policyId; Reason = reason;
    }
}