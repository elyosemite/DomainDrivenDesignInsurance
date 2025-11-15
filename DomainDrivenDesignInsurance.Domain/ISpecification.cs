namespace DomainDrivenDesignInsurance.Domain;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    string? Message { get; }
}
