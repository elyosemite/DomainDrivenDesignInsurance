namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class PolicyAlreadyCancelledException : Exception
{
    public PolicyAlreadyCancelledException()
        : base("The policy has already been cancelled.")
    {
    }

    public PolicyAlreadyCancelledException(string message)
        : base(message)
    {
    }

    public PolicyAlreadyCancelledException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}