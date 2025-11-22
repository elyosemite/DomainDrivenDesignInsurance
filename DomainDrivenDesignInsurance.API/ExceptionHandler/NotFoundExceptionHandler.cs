using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        _logger.LogWarning(
            exception,
            "NotFoundException handled: {Message}",
            notFoundException.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "Not Found",
            Detail = notFoundException.Message,
            Status = StatusCodes.Status404NotFound
        };

        await httpContext.Response
            .WriteAsJsonAsync(
                problemDetails,
                cancellationToken: cancellationToken);

        return true;
    }
}