namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class PremiumCalculationViolationException : DomainException
{
    public PremiumCalculationViolationException(string message)
        : base(message)
    {
    }

    public PremiumCalculationViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}