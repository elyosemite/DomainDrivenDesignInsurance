namespace DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;

/// <summary>
/// Period value object - validity interval
/// </summary>
public class Period : IEquatable<Period>
{
    public DateTime From { get; }
    public DateTime To { get; }

    public Period(DateTime from, DateTime to)
    {
        if (to <= from) throw new ArgumentException("Period 'to' must be after 'from'");
        From = from;
        To = to;
    }

    public bool Includes(DateTime dt) => dt >= From && dt <= To;
    public bool Overlaps(Period other) => !(other.To <= From || other.From >= To);

    public override bool Equals(object? obj) => Equals(obj as Period);
    public bool Equals(Period? other) => other is not null && From == other.From && To == other.To;
    public override int GetHashCode() => HashCode.Combine(From, To);
}

/// <summary>
/// Null Object Pattern for Period
/// </summary>
public class NullObjectPeriod : Period
{
    private static readonly NullObjectPeriod _instance = new NullObjectPeriod();

    private NullObjectPeriod() : base(DateTime.MinValue, DateTime.MinValue) { }

    public static NullObjectPeriod Instance => _instance;
}