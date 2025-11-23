using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class PremiumCalculationViolationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<PremiumCalculationViolationExceptionHandler> _logger;

    public PremiumCalculationViolationExceptionHandler(ILogger<PremiumCalculationViolationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not PremiumCalculationViolationException premiumCalculationViolationException)
        {
            return false;
        }

        _logger.LogWarning(
            premiumCalculationViolationException,
            "A premium calculation violation exception occurred: {Message}",
            premiumCalculationViolationException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Premium calculation violation",
            Status = StatusCodes.Status400BadRequest,
            Detail = premiumCalculationViolationException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}