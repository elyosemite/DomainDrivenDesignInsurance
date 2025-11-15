using DomainDrivenDesignInsurance.Application.Interfaces;

namespace DomainDrivenDesignInsurance.Application.CommandHandler;

public class CancelPolicyCommandHandler
{
    private readonly IPolicyRepository _policyRepository;


    public CancelPolicyCommandHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }


    public async Task Handle(CancelPolicyCommand cmd)
    {
        var policy = await _policyRepository.GetAsync(cmd.PolicyId)
        ?? throw new KeyNotFoundException("Policy not found");


        policy.Cancel(cmd.Reason);


        await _policyRepository.UpdateAsync(policy);
        await _policyRepository.SaveChangesAsync();
    }
}
