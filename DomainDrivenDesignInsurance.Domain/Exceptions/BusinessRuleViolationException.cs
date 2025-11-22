namespace DomainDrivenDesignInsurance.Domain.Exceptions;

// Business rule violations
public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string rule, string message)
        : base(message)
    {
        Rule = rule;
    }

    public string Rule { get; }
}
