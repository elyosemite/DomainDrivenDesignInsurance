using System.Collections.Concurrent;
using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;

namespace DomainDrivenDesignInsurance.Infrastructure.Repositories
{
    public class PolicyRepositoryInMemory : IPolicyRepository
    {
        private readonly ConcurrentDictionary<Guid, Policy> _policies = new();

        public Task<Policy?> GetAsync(Guid id)
        {
            _policies.TryGetValue(id, out var policy);
            return Task.FromResult(policy);
        }

        public Task AddAsync(Policy policy)
        {
            _policies[policy.Id] = policy;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Policy policy)
        {
            _policies[policy.Id] = policy;
            return Task.CompletedTask;
        }
    }
}