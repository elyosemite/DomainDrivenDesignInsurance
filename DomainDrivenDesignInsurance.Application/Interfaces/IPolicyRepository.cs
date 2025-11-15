
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Application.Interfaces;

public interface IPolicyRepository
{
    Task<Policy?> GetAsync(Guid id);
    Task AddAsync(Policy policy);
    Task UpdateAsync(Policy policy);
}
