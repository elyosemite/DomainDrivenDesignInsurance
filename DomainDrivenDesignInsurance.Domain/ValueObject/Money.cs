namespace DomainDrivenDesignInsurance.Domain.ValueObject;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0) throw new ArgumentException("Money amount cannot be negative", nameof(amount));
        Amount = amount;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }


    private void EnsureSameCurrency(Money other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        if (!Currency.Equals(other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Cannot operate on different currencies");
    }

    public override bool Equals(object? obj) => Equals(obj as Money);
    public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Currency} {Amount:F2}";
}

public sealed class NullMoney : Money
{
    public static readonly NullMoney Instance = new NullMoney();

    private NullMoney() : base(0, string.Empty) { }

    public override string ToString() => "No Money";

    public override bool Equals(object? obj) => obj is NullMoney;
    public override int GetHashCode() => 0;
}