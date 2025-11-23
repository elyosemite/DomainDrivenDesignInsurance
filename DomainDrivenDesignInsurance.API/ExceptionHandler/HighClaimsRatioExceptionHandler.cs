using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public sealed class HighClaimsRatioExceptionHandler : IExceptionHandler
{
    private readonly ILogger<HighClaimsRatioExceptionHandler> _logger;

    public HighClaimsRatioExceptionHandler(ILogger<HighClaimsRatioExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not HighClaimsRatioException highClaimsRatioException)
        {
            return false;
        }

        _logger.LogWarning(
            "High claims ratio detected for policy {PolicyNumber} with ratio {ClaimsRatio}",
            highClaimsRatioException.PolicyNumber,
            highClaimsRatioException.ClaimsRatio);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Title = "High Claims Ratio",
            Detail = $"The claims ratio for policy {highClaimsRatioException.PolicyNumber} is {highClaimsRatioException.ClaimsRatio:P}, which exceeds the acceptable limit.",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://example.com/probs/high-claims-ratio"
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}