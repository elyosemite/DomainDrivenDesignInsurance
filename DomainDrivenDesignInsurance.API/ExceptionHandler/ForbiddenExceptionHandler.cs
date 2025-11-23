using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class ForbiddenExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ForbiddenExceptionHandler> _logger;

    public ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbiddenException)
        {
            return false;
        }

        _logger.LogWarning(
            forbiddenException,
            "A forbidden exception occurred: {Message}",
            forbiddenException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Forbidden",
            Status = StatusCodes.Status403Forbidden,
            Detail = forbiddenException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken)
            .ContinueWith(_ => true, cancellationToken);

        return true;
    }
}