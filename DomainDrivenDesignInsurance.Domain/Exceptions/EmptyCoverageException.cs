namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class EmptyCoverageException : DomainException
{
    public EmptyCoverageException()
        : base("Policy must have at least one coverage")
    {
    }

    public EmptyCoverageException(string message)
        : base(message)
    {
    }

    public EmptyCoverageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}