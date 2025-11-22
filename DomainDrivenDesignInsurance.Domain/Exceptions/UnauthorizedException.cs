namespace DomainDrivenDesignInsurance.Domain.Exceptions;

// Authorization errors
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Unauthorized") 
        : base(message) { }
}
