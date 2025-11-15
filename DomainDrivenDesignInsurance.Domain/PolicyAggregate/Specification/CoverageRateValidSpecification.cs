using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class CoverageRateValidSpecification : ISpecification<Policy>
{
    public string? Message => "All coverage rates must be equal or greater than zero.";

    public bool IsSatisfiedBy(Policy policy)
    {
        return policy.Coverages.All(c => c.Rate >= 0);
    }
}