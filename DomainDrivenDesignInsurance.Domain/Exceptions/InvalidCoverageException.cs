namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class InvalidCoverageException : Exception
{
    public InvalidCoverageException() 
        : base("The provided coverage is invalid.") { }

    public InvalidCoverageException(string message) 
        : base(message) { }

    public InvalidCoverageException(string message, Exception innerException) 
        : base(message, innerException) { }
}