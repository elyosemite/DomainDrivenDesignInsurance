using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;
using Mediator;

namespace DomainDrivenDesignInsurance.Application.Commands;

public class IssuePolicyCommandRequest : ICommand<IssuePolicyCommandResponse>
{
    public Guid PolicyId { get; set; }
    public string PolicyHolderName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Value { get; set; }
}

public class IssuePolicyCommandResponse
{
    public Guid PolicyId { get; set; }
}

public class IssuePolicyHandler : ICommandHandler<IssuePolicyCommandRequest, IssuePolicyCommandResponse>
{
    private readonly IPolicyRepository _policyRepository;

    public IssuePolicyHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async ValueTask<IssuePolicyCommandResponse> Handle(
        IssuePolicyCommandRequest request,
        CancellationToken cancellationToken)
    {
        var insuredId = Guid.NewGuid();
        var brokerId = Guid.NewGuid();
        var period = new Period(request.StartDate, request.EndDate);
        var random = new Random();
        var coverages = new List<Coverage>
        {
            new Coverage("COV" + random.Next(1000, 9999), "Standard Coverage", new Money(request.Value, "USD"), 0.05m),
            new Coverage("COV" + random.Next(1000, 9999), "Additional Coverage", new Money(request.Value / 2, "USD"), 0.03m),
            new Coverage("COV" + random.Next(1000, 9999), "Premium Coverage", new Money(request.Value * 2, "USD"), 0.07m),
            new Coverage("COV" + random.Next(1000, 9999), "Basic Coverage", new Money(request.Value / 4, "USD"), 0.02m),
            new Coverage("COV" + random.Next(1000, 9999), "Extended Coverage", new Money(request.Value * 1.5m, "USD"), 0.06m)
        };

        var policy = Policy.Issue(
            insuredId,
            request.PolicyHolderName,
            brokerId,
            period,
            coverages
        );

        await _policyRepository.AddAsync(policy);

        return new IssuePolicyCommandResponse
        {
            PolicyId = policy.Id
        };
    }
}
