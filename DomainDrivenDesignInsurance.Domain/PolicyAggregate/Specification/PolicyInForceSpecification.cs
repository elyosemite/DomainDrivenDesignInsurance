using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class PolicyInForceSpecification : ISpecification<Policy>
{
    public string? Message => "Policy is not currently in force.";

    public bool IsSatisfiedBy(Policy policy)
    {
        var now = DateTime.UtcNow;
        return policy.Period.Includes(now);
    }
}