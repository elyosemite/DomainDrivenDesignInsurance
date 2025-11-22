using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class InvalidPeriodPolicyExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidPeriodPolicyExceptionHandler> _logger;

    public InvalidPeriodPolicyExceptionHandler(ILogger<InvalidPeriodPolicyExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidPeriodPolicyException invalidPeriodException)
        {
            return false;
        }

        _logger.LogWarning(
            invalidPeriodException,
            "An invalid period exception occurred: {Message}",
            invalidPeriodException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Invalid Period",
            Status = StatusCodes.Status400BadRequest,
            Detail = invalidPeriodException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}