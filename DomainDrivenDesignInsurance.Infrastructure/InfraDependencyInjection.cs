using DomainDrivenDesignInsurance.Application.Interfaces;
using DomainDrivenDesignInsurance.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DomainDrivenDesignInsurance.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IPolicyRepository, PolicyRepositoryInMemory>();
        return services;
    }
}