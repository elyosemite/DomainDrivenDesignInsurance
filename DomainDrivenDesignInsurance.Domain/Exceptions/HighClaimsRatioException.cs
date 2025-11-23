namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class HighClaimsRatioException : Exception
{
    public string PolicyNumber { get; init; } = string.Empty;
    public double ClaimsRatio { get; init; }
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