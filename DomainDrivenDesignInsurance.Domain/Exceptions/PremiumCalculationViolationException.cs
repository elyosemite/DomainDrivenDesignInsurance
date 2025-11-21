namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class PremiumCalculationViolationException : Exception
{
    public PremiumCalculationViolationException()
    {
    }

    public PremiumCalculationViolationException(string message)
        : base(message)
    {
    }

    public PremiumCalculationViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}