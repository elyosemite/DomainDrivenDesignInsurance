using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;

namespace DomainDrivenDesignInsurance.Infrastructure.Data.Mappings;

public static class PolicyMappingExtensions
{
    public static PolicyModel ToDataModel(this Policy policy)
    {
        return new PolicyModel
        {
            Id = policy.Id,
            InsuredId = policy.InsuredId,
            BrokerId = policy.BrokerId,
            Status = (int)policy.Status,
            PeriodFrom = policy.Period.From,
            PeriodTo = policy.Period.To,
            Coverages = policy.Coverages.Select(c => c.ToDataModel(policy.Id)).ToList(),
            Endorsements = policy.Endorsements.Select(e => e.ToDataModel(policy.Id)).ToList()
        };
    }

    public static Policy ToDomain(this PolicyModel model)
    {
        var policy = Policy.Issue(
            model.Id,
            model.InsuredId,
            model.placeHolderName,
            model.BrokerId,
            new Period(model.PeriodFrom, model.PeriodTo),
            model.Coverages.Select(c => c.ToDomain())
        );

        // reinserir endorsements
        foreach (var e in model.Endorsements)
            policy.CreateEndorsement(e.Type, e.EffectiveDate, e.ToDomain().PremiumDelta);

        return policy;
    }
}
