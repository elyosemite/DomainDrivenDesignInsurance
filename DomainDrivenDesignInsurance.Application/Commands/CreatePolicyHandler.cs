using Mediator;

namespace DomainDrivenDesignInsurance.Application.Commands;

public class CreatePolicyRequest : IRequest<CreatePolicyResponse>
{
    public Guid PolicyId { get; set; }
    public string PolicyHolderName { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class CreatePolicyResponse
{
    public Guid PolicyId { get; set; }
}

public class CreatePolicyHandler : IRequestHandler<CreatePolicyRequest, CreatePolicyResponse>
{
    private readonly List<CreatePolicyRequest> _policies = new();

    public async ValueTask<CreatePolicyResponse> Handle(CreatePolicyRequest request, CancellationToken cancellationToken)
    {
        var policy = new CreatePolicyResponse
        {
            PolicyId = request.PolicyId
        };

        _policies.Add(request);

        return policy;
    }
}

