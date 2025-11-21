using DomainDrivenDesignInsurance.Application.Commands;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace DomainDrivenDesignInsurance.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(
            (MediatorOptions options) => 
            {
                options.Assemblies = [typeof(IssuePolicyCommandRequest)];
                options.ServiceLifetime = ServiceLifetime.Scoped;
            }
        );
        return services;
    }
}