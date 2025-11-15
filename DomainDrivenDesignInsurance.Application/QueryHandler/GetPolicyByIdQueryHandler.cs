using DomainDrivenDesignInsurance.Application.Interfaces;

namespace DomainDrivenDesignInsurance.Application;

public record GetPolicyByIdQuery(Guid PolicyId);
public record GetPoliciesByInsuredQuery(Guid InsuredId);
public record PolicyDto(
    Guid Id,
    Guid InsuredId,
    Guid BrokerId,
    string Status,
    DateTime From,
    DateTime To,
    List<CoverageDto> Coverages,
    List<EndorsementDto> Endorsements);
public record CoverageDto(string Code, string Description, decimal SumInsured, decimal Rate);
public record EndorsementDto(string Type, DateTime EffectiveDate, decimal PremiumDelta);

public class GetPolicyByIdQueryHandler
{
    private readonly IPolicyRepository _policyRepository;

    public GetPolicyByIdQueryHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<PolicyDto?> Handle(GetPolicyByIdQuery query)
    {
        var policy = await _policyRepository.GetAsync(query.PolicyId);
        if (policy == null) return null;


        return new PolicyDto(
        policy.Id,
        policy.InsuredId,
        policy.BrokerId,
        policy.Status.ToString(),
        policy.Period.From,
        policy.Period.To,
        policy.Coverages.Select(c => new CoverageDto(c.CoverageCode, c.Description, c.SumInsured.Amount, c.Rate)).ToList(),
        policy.Endorsements.Select(e => new EndorsementDto(e.Type, e.EffectiveDate, e.PremiumDelta.Amount)).ToList()
        );
    }
}