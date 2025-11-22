using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred. {Mesage}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Server error",
            Status = StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

public static class GlobalExceptionHandlerExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(
        this IServiceCollection service)
    {
        service.AddExceptionHandler<BadRequestExceptionHandler>();
        service.AddExceptionHandler<NotFoundExceptionHandler>();
        service.AddExceptionHandler<InvalidPeriodPolicyExceptionHandler>();
        service.AddExceptionHandler<InvalidInsuredNameExceptionHandler>();
        service.AddExceptionHandler<GlobalExceptionHandler>();
        return service;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
