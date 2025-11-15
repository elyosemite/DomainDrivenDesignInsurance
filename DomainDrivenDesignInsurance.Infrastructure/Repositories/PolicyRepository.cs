using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Infrastructure.Data.Context;
using DomainDrivenDesignInsurance.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignInsurance.Infrastructure.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly InsuranceDbContext _ctx;

    public PolicyRepository(InsuranceDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(Policy policy)
    {
        var model = policy.ToDataModel();
        await _ctx.Policies.AddAsync(model);
    }

    public async Task<Policy?> GetAsync(Guid id)
    {
        var model = await _ctx.Policies
            .Include(p => p.Coverages)
            .Include(p => p.Endorsements)
            .FirstOrDefaultAsync(p => p.Id == id);

        return model?.ToDomain();
    }

    public async Task UpdateAsync(Policy policy)
    {
        var model = policy.ToDataModel();
        _ctx.Policies.Update(model);
    }

    public async Task DeleteAsync(Guid id)
    {
        var model = await _ctx.Policies.FindAsync(id);
        if (model != null)
            _ctx.Policies.Remove(model);
    }
}
