using DomainDrivenDesignInsurance.Application.Interfaces;
using Mediator;

namespace DomainDrivenDesignInsurance.Application.Queries;

public sealed record GetPolicyByIdQueryRequest(Guid PolicyId) : IQuery<GetPolicyByIdQueryResponse>;
public sealed record GetPolicyByIdQueryResponse(
    Guid PolicyId,
    string PolicyHolderName,
    string Status,
    decimal TotalPremiumAmount,
    string TotalPremiumCurrency
);

public sealed class GetPolicyQueryHandler : IQueryHandler<GetPolicyByIdQueryRequest, GetPolicyByIdQueryResponse>
{
    private readonly IPolicyRepository _policyRepository;

    public GetPolicyQueryHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async ValueTask<GetPolicyByIdQueryResponse> Handle(GetPolicyByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetAsync(request.PolicyId);
        if (policy is null)
        {
            throw new KeyNotFoundException($"Policy with ID {request.PolicyId} not found.");
        }

        return new GetPolicyByIdQueryResponse(
            policy.Id,
            policy.PolicyHolderName,
            policy.Status.ToString(),
            policy.TotalPremium.Amount,
            policy.TotalPremium.Currency
        );
    }
}