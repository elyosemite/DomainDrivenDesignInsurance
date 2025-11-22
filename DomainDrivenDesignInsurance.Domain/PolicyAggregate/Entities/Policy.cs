using DomainDrivenDesignInsurance.Domain.Exceptions;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.DomainEvents;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Enums;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;
using System.Collections.ObjectModel;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

/// <summary>
/// Aggregate root: Policy
/// </summary>
public class Policy : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid InsuredId { get; private set; }
    public string PolicyHolderName { get; private set; } = string.Empty;
    public Guid BrokerId { get; private set; }
    public PolicyStatus Status { get; private set; }
    public Period Period { get; private set; } = NullObjectPeriod.Instance;
    public Money TotalPremium { get; private set; } = NullMoney.Instance;

    // Value objects
    private readonly List<Coverage> _coverages = new();
    public IReadOnlyCollection<Coverage> Coverages => new ReadOnlyCollection<Coverage>(_coverages);

    // Endorsements as entities within aggregate
    private readonly List<Endorsement> _endorsements = new();
    public IReadOnlyCollection<Endorsement> Endorsements => new ReadOnlyCollection<Endorsement>(_endorsements);

    // Domain events attached to this aggregate instance
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Factory for creating a new policy from a quotation or issuance
    public static Policy Issue(Guid id, Guid insuredId, string placeHolderName, Guid brokerId, Period period, IEnumerable<Coverage> coverages)
    {
        if (string.IsNullOrWhiteSpace(placeHolderName)) throw new ArgumentNullException(nameof(placeHolderName));
        if (period == null) throw new InvalidPeriodPolicyException(nameof(period));
        if (coverages == null || !coverages.Any()) throw new ArgumentException("Policy must have at least one coverage", nameof(coverages));

        var p = new Policy
        {
            PolicyHolderName = placeHolderName,
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            InsuredId = insuredId,
            BrokerId = brokerId,
            Status = PolicyStatus.Active,
            Period = period
        };

        p.AddCoverageRange(coverages);
        p.TotalPremium = p.CalculateTotalPremium();

        // raise domain event
        p.AddDomainEvent(new PolicyIssued(p.Id, p.InsuredId, p.BrokerId));

        return p;
    }

    // Factory overload for simpler policy issuance
    public static Policy Issue(Guid insuredId, string policyHolderName, Guid brokerId, Period period, IEnumerable<Coverage> coverages)
    {
        if (string.IsNullOrWhiteSpace(policyHolderName)) throw new ArgumentNullException(nameof(policyHolderName));
        if (period == null) throw new ArgumentNullException(nameof(period));
        if (coverages == null || !coverages.Any()) throw new ArgumentException("Policy must have at least one coverage", nameof(coverages));

        var id = Guid.NewGuid();

        var policy = new Policy
        {
            PolicyHolderName = policyHolderName,
            Id = id,
            InsuredId = insuredId,
            BrokerId = brokerId,
            Status = PolicyStatus.Active,
            Period = period
        };
        
        policy.AddCoverageRange(coverages);
        policy.TotalPremium = policy.CalculateTotalPremium();

        policy.AddDomainEvent(new PolicyIssued(policy.Id, policy.InsuredId, policy.BrokerId));

        return policy;
    }

    public Money CalculateTotalPremium()
    {
        var subtotal = _coverages.Select(c => c.CalculatePremium().Amount).Sum();

        if (_coverages.Count <= 0) throw new InvalidCoverageException("Cannot calculate premium for a policy with no coverages. It is recommended to add at least one coverage.");
        if (subtotal <= 0) throw new PremiumCalculationViolationException("Calculated premium cannot be zero or negative. Please review coverages and their sum insured amounts.");

        TotalPremium = new Money(subtotal, _coverages.First().SumInsured.Currency);

        return TotalPremium;
    }

    // Business operation: create an endorsement
    public Endorsement CreateEndorsement(string type, DateTime effectiveDate, Money premiumDelta)
    {
        if (Status != PolicyStatus.Active)
            throw new InvalidOperationException("Only active policies can receive endorsements");

        if (!Period.Includes(effectiveDate))
            throw new InvalidOperationException("Endorsement effective date must be within policy period");

        // more invariants (e.g., endorsement overlaps) can be added here

        var endorsement = new Endorsement(type, effectiveDate, premiumDelta);
        _endorsements.Add(endorsement);

        // apply premium change to the policy as side-effect (if domain requires)
        // note: in this domain object we keep premium calculation based on coverages, but
        // you might store an aggregated premium state and update it here.

        AddDomainEvent(new EndorsementCreated(Id, endorsement.Id, InsuredId, type, premiumDelta.Amount));

        return endorsement;
    }

    public void AddCoverage(Coverage coverage)
    {
        if (coverage == null) throw new ArgumentNullException(nameof(coverage));
        _coverages.Add(coverage);
    }

    public void AddCoverageRange(IEnumerable<Coverage> coverages)
    {
        if (coverages == null || !coverages.Any()) throw new ArgumentException("Coverages collection cannot be null or empty", nameof(coverages));
        _coverages.AddRange(coverages);
    }

    public void Cancel(string reason)
    {
        if (Status == PolicyStatus.Cancelled) return;
        Status = PolicyStatus.Cancelled;
        AddDomainEvent(new PolicyCancelled(Id, reason));
    }

    // Domain Events helpers
    private void AddDomainEvent(DomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
