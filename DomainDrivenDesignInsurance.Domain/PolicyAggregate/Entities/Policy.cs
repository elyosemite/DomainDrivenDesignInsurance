using System.Collections.ObjectModel;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.DomainEvents;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Enums;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

/// <summary>
/// Aggregate root: Policy
/// </summary>
public class Policy : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid InsuredId { get; private set; }
    public Guid BrokerId { get; private set; }
    public PolicyStatus Status { get; private set; }
    public Period Period { get; private set; }

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
    public static Policy Issue(Guid id, Guid insuredId, Guid brokerId, Period period, IEnumerable<Coverage> coverages)
    {
        if (period == null) throw new ArgumentNullException(nameof(period));
        if (coverages == null || !coverages.Any()) throw new ArgumentException("Policy must have at least one coverage", nameof(coverages));

        var p = new Policy
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            InsuredId = insuredId,
            BrokerId = brokerId,
            Status = PolicyStatus.Active,
            Period = period
        };

        p._coverages.AddRange(coverages);

        // raise domain event
        p.AddDomainEvent(new PolicyIssued(p.Id, p.InsuredId, p.BrokerId));

        return p;
    }

    public Money CalculateTotalPremium()
    {
        var subtotal = _coverages.Select(c => c.CalculatePremium().Amount).Sum();
        // example: no taxes in domain object
        return new Money(subtotal, _coverages.First().SumInsured.Currency);
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
