using DomainDrivenDesignInsurance.API.Middleware;
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

        services.AddProblemDetails();
        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseAuthorization();
        app.UseFastEndpoints();
        app.UseSwaggerGen();
        app.MapOpenApi();


        // Middlewares
        app.UseExceptionHandlingMiddleware();
        return app;
    }
  
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
    {
        return builder
            .UseMiddleware<GlobalExceptionHandlingMiddleware>()
            .UseMiddleware<ProblemDetailsMiddleware>()
            .UseMiddleware<EnrichedExceptionHandlerMiddleware>();
    }
}