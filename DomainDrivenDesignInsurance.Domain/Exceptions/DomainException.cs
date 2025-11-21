namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public Dictionary<string, string[]> Errors { get; private set; } = new();
    protected DomainException(string message) : base(message)
    {
    }

    public void SetErrors(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

// Validation errors
public class ValidationException : DomainException
{
    public ValidationException()
        : base("One or more validation errors occurred")
    {
    }
}

// Not found errors
public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found")
    {
        EntityName = entityName;
        Key = key;
    }

    public string EntityName { get; }
    public object Key { get; }
}

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

// Authorization errors
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Unauthorized") 
        : base(message) { }
}

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "Forbidden") 
        : base(message) { }
}