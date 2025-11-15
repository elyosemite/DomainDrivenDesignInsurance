using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Enums;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Cannot_Issue_Policy_With_No_Coverages()
    {
        var period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));


        Assert.Throws<ArgumentException>(() =>
        Policy.Issue(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period, new List<Coverage>()));
    }

    [Test]
    public void Issue_Policy_With_Valid_Coverages_Should_Work()
    {
        var period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        var coverages = new List<Coverage>
        {
            new Coverage("BIKE", "Bike Coverage", new Money(2000), 0.10m)
        };


        var policy = Policy.Issue(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period, coverages);


        Assert.That(policy.Status, Is.EqualTo(PolicyStatus.Active));
        Assert.That(policy.Coverages.Count, Is.EqualTo(1));
    }

    [Test]
    public void Cannot_Add_Endorsement_When_Policy_Is_Cancelled()
    {
        var period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        var coverages = new List<Coverage>
        {
            new Coverage("BIKE", "Bike Coverage", new Money(2000), 0.10m)
        };


        var policy = Policy.Issue(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period, coverages);


        policy.Cancel("Non-payment");


        Assert.Throws<InvalidOperationException>(() =>
        policy.CreateEndorsement("Upgrade", DateTime.UtcNow.AddDays(1), new Money(50)));
    }

    [Test]
    public void Cannot_Add_Endorsement_Outside_Policy_Period()
    {
        var period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
        var coverages = new List<Coverage>
        {
            new Coverage("BIKE", "Bike Coverage", new Money(2000), 0.10m)
        };


        var policy = Policy.Issue(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period, coverages);


        var outsideDate = DateTime.UtcNow.AddDays(20);


        Assert.Throws<InvalidOperationException>(() =>
        policy.CreateEndorsement("Address Change", outsideDate, new Money(0)));
    }

    [Test]
    public void Can_Create_Valid_Endorsement_Within_Period()
    {
        var period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
        var coverages = new List<Coverage>
        {
            new Coverage("BIKE", "Bike Coverage", new Money(2000), 0.10m)
        };


        var policy = Policy.Issue(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), period, coverages);


        var endorsement = policy.CreateEndorsement("Accessory Upgrade", DateTime.UtcNow.AddDays(2), new Money(100));


        Assert.That(policy.Endorsements.Count, Is.EqualTo(1));
        Assert.That(endorsement.Type, Is.EqualTo("Accessory Upgrade"));
    }
}
