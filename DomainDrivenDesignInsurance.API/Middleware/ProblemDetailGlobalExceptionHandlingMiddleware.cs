using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.Middleware;

public class ProblemDetailGlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        } 
        catch(Exception exception)
        {
            logger.LogError(exception, "An unhandled exception occurred.");

            await HandleExceptionAsync(context, exception);
        }
    }

    public sealed record ErrorDetails(int StatusCode, string Message, Dictionary<string, string[]>? Errors);

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            EmptyCoverageException => StatusCodes.Status400BadRequest,
            HighClaimsRatioException => StatusCodes.Status400BadRequest,
            InvalidCoverageException => StatusCodes.Status400BadRequest,
            InvalidPeriodPolicyException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            PolicyAlreadyCancelledException => StatusCodes.Status400BadRequest,
            PolicyOutOfValidityPeriodException => StatusCodes.Status400BadRequest,
            PremiumCalculationViolationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().ToString(),
                Status = context.Response.StatusCode,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message
            }
        });
    }
}

public static class ProblemDetailGlobalExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseProblemDetailGlobalExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ProblemDetailGlobalExceptionHandlingMiddleware>();
    }
}