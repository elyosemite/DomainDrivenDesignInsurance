namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class InvalidPeriodPolicyException : DomainException
{
    public InvalidPeriodPolicyException()
        : base("The policy period is invalid.")
    {
    }

    public InvalidPeriodPolicyException(string message)
        : base(message)
    {
    }

    public InvalidPeriodPolicyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}