using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;
using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Infrastructure.Data.Mappings;

public static class EndorsementMappingExtensions
{
    public static EndorsementModel ToDataModel(this Endorsement endorsement, Guid policyId)
    {
        return new EndorsementModel
        {
            Id = endorsement.Id,
            PolicyId = policyId,
            Type = endorsement.Type,
            EffectiveDate = endorsement.EffectiveDate,
            PremiumDeltaAmount = endorsement.PremiumDelta.Amount,
            Currency = endorsement.PremiumDelta.Currency,
            CreatedAt = endorsement.CreatedAt
        };
    }

    public static Endorsement ToDomain(this EndorsementModel model)
    {
        return new Endorsement(
            model.Type,
            model.EffectiveDate,
            new Money(model.PremiumDeltaAmount, model.Currency)
        );
    }
}
