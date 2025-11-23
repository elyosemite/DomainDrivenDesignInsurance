using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class EmptyCoverageExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EmptyCoverageException emptyCoverageException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Title = "Empty coverage error",
            Status = StatusCodes.Status400BadRequest,
            Detail = emptyCoverageException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}