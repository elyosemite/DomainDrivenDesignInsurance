namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Specification;

public class EndorsementTypeAllowedSpecification : ISpecification<string>
{
    private static readonly HashSet<string> AllowedTypes = new()
    {
        "AddressChange",
        "CoverageIncrease",
        "CoverageDecrease",
        "CustomerDataUpdate"
    };

    public string? Message => "Endorsement type is not allowed.";

    public bool IsSatisfiedBy(string endorsementType)
    {
        return AllowedTypes.Contains(endorsementType);
    }
}