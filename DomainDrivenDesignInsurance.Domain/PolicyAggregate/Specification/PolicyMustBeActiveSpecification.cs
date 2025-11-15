using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Enums;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class PolicyMustBeActiveSpecification : ISpecification<Policy>
{
    public string? Message => "Policy must be active.";

    public bool IsSatisfiedBy(Policy policy)
    {
        return policy.Status == PolicyStatus.Active;
    }
}