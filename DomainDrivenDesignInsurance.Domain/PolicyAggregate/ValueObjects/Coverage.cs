using DomainDrivenDesignInsurance.Domain.ValueObject;

namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;

/// <summary>
/// Coverage value object - represents a coverage line within a policy
/// </summary>
public sealed class Coverage : IEquatable<Coverage>
{
    public string CoverageCode { get; }
    public string Description { get; }

    /// <summary>
    /// 
    /// </summary>
    public Money SumInsured { get; }
    public decimal Rate { get; } // percent as 0.10 for 10%

    public Coverage(string coverageCode, string description, Money sumInsured, decimal rate)
    {
        CoverageCode = coverageCode ?? throw new ArgumentNullException(nameof(coverageCode));
        Description = description ?? string.Empty;
        SumInsured = sumInsured ?? throw new ArgumentNullException(nameof(sumInsured));
        if (rate < 0) throw new ArgumentException("rate must be >= 0", nameof(rate));
        Rate = rate;
    }

    public Money CalculatePremium()
    {
        var premium = SumInsured.Amount * Rate;
        return new Money(premium, SumInsured.Currency);
    }

    public override bool Equals(object? obj) => Equals(obj as Coverage);
    public bool Equals(Coverage? other) => other is not null && CoverageCode == other.CoverageCode;
    public override int GetHashCode() => CoverageCode.GetHashCode();
}