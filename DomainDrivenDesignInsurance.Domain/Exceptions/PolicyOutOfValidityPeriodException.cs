namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class PolicyOutOfValidityPeriodException : Exception
{
    public PolicyOutOfValidityPeriodException()
        : base("The policy is out of its validity period.")
    {
    }

    public PolicyOutOfValidityPeriodException(string message)
        : base(message)
    {
    }

    public PolicyOutOfValidityPeriodException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}