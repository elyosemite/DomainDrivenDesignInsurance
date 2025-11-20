namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class HighClaimsRatioException : Exception
{
    public HighClaimsRatioException()
    {
    }

    public HighClaimsRatioException(string message) : base(message)
    {
    }

    public HighClaimsRatioException(string message, Exception innerException) : base(message, innerException)
    {
    }
}