using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;

namespace DomainDrivenDesignInsurance.Infrastructure.Data.Mappings;

public static class CoverageMappingExtensions
{
    public static CoverageModel ToDataModel(this Coverage coverage, Guid policyId)
    {
        return new CoverageModel
        {
            Id = Guid.NewGuid(),
            PolicyId = policyId,
            CoverageCode = coverage.CoverageCode,
            Description = coverage.Description,
            SumInsuredAmount = coverage.SumInsured.Amount,
            Currency = coverage.SumInsured.Currency,
            Rate = coverage.Rate
        };
    }

    public static Coverage ToDomain(this CoverageModel model)
    {
        return new Coverage(
            model.CoverageCode,
            model.Description,
            new Money(model.SumInsuredAmount, model.Currency),
            model.Rate
        );
    }
}
