namespace DomainDrivenDesignInsurance.Domain.ValueObject;

public sealed class PersonName : IEquatable<PersonName>
{
    public string FirstName { get; }
    public string LastName { get; }


    public PersonName(string firstName, string lastName)
    {
        FirstName = firstName?.Trim() ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName?.Trim() ?? throw new ArgumentNullException(nameof(lastName));
    }


    public override bool Equals(object? obj) => Equals(obj as PersonName);
    public bool Equals(PersonName? other) => other is not null && FirstName == other.FirstName && LastName == other.LastName;
    public override int GetHashCode() => HashCode.Combine(FirstName, LastName);
    public override string ToString() => $"{FirstName} {LastName}";
}
