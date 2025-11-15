using DomainDrivenDesignInsurance.Domain.Specifications;

namespace DomainDrivenDesignInsurance.Domain.Specifications;

public class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public string? Message => $"{_left.Message}; {_right.Message}";

    public bool IsSatisfiedBy(T entity)
    {
        return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
    }
}
