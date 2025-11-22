namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "Forbidden") 
        : base(message) { }
}