using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Application.CommandHandler;

public class CreatePolicyCommandHandler
{
    private readonly IPolicyRepository _policyRepository;

    public CreatePolicyCommandHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<Guid> Handle(CreatePolicyCommand cmd)
    {
        var period = new Period(cmd.PeriodFrom, cmd.PeriodTo);


        var coverages = cmd.Coverages
            .Select(c => new Coverage(
                c.CoverageCode,
                c.Description,
                new Money(c.SumInsured),
                c.Rate))
            .ToList();


        var policy = Policy.Issue(cmd.PolicyId, cmd.InsuredId, cmd.BrokerId, period, coverages);

        await _policyRepository.AddAsync(policy);


        return policy.Id;
    }
}
