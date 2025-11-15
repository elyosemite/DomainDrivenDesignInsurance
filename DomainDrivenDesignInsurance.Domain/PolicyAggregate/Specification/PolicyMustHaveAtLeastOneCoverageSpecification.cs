using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class PolicyMustHaveAtLeastOneCoverageSpecification : ISpecification<Policy>
{
    public string? Message => "Policy must have at least one coverage.";

    public bool IsSatisfiedBy(Policy policy)
    {
        return policy.Coverages.Any();
    }
}