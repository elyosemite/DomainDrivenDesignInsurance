using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class EndorsementEffectiveDateWithinPolicyPeriodSpecification : ISpecification<Policy>
{
    private readonly DateTime _effectiveDate;

    public EndorsementEffectiveDateWithinPolicyPeriodSpecification(DateTime effectiveDate)
    {
        _effectiveDate = effectiveDate;
    }

    public string? Message => "Endorsement effective date must be within policy period.";

    public bool IsSatisfiedBy(Policy policy)
    {
        return policy.Period.Includes(_effectiveDate);
    }
}