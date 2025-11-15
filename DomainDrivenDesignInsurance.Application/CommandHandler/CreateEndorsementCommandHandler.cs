using System;
using System.Collections.Generic;
using System.Text;
using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Application.CommandHandler;

public class CreateEndorsementCommandHandler
{
    private readonly IPolicyRepository _policyRepository;


    public CreateEndorsementCommandHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<Guid> Handle(CreateEndorsementCommand cmd)
    {
        var policy = await _policyRepository.GetAsync(cmd.PolicyId)
            ?? throw new KeyNotFoundException("Policy not found");


        var endorsement = policy.CreateEndorsement(
            cmd.Type,
            cmd.EffectiveDate,
            new Money(cmd.PremiumDelta)
        );


        await _policyRepository.UpdateAsync(policy);
        await _policyRepository.SaveChangesAsync();


        return endorsement.Id;
    }
}
