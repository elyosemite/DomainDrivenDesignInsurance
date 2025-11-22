namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class InvalidInsuredNameException : DomainException
{
    public InvalidInsuredNameException()
        : base("The insured name is invalid.")
    {
    }

    public InvalidInsuredNameException(string message)
        : base(message)
    {
    }

    public InvalidInsuredNameException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}