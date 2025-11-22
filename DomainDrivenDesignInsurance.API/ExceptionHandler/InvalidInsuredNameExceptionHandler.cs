using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class InvalidInsuredNameExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidInsuredNameExceptionHandler> _logger;

    public InvalidInsuredNameExceptionHandler(ILogger<InvalidInsuredNameExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidInsuredNameException invalidInsuredNameException)
        {
            return false;
        }

        _logger.LogWarning(
            invalidInsuredNameException,
            "An invalid insured name exception occurred: {Message}",
            invalidInsuredNameException.Message);
        
        var problemDetails = new ProblemDetails
        {
            Title = "Invalid Insured Name",
            Status = StatusCodes.Status400BadRequest,
            Detail = invalidInsuredNameException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}