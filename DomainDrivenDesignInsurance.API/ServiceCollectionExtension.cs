using DomainDrivenDesignInsurance.Application;
using DomainDrivenDesignInsurance.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace DomainDrivenDesignInsurance.API;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();
        services.AddFastEndpoints();
        services.SwaggerDocument();
        services.AddOpenApi();
        services.AddAuthorization();
        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseAuthorization();
        app.UseFastEndpoints();
        app.UseSwaggerGen();
        app.MapOpenApi();
        return app;
    }
}