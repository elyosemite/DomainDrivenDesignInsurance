namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public Dictionary<string, string[]> Errors { get; private set; } = new();
    protected DomainException(string message) : base(message)
    {
    }

    public void SetErrors(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
