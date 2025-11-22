namespace DomainDrivenDesignInsurance.Domain.Exceptions;

// Validation errors
public class ValidationException : DomainException
{
    public ValidationException()
        : base("One or more validation errors occurred")
    {
    }
}
