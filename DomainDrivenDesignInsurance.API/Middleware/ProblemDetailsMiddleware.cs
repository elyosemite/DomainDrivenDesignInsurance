using DomainDrivenDesignInsurance.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.Middleware;

public class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;

    public ProblemDetailsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationEx => new ValidationProblemDetails(validationEx.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Instance = context.Request.Path
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = notFoundEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Instance = context.Request.Path,
                Extensions =
                {
                    ["entityName"] = notFoundEx.EntityName,
                    ["key"] = notFoundEx.Key
                }
            },
            BusinessRuleViolationException businessEx => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Business rule violation",
                Detail = businessEx.Message,
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                Instance = context.Request.Path,
                Extensions =
                {
                    ["rule"] = businessEx.Rule
                }
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Instance = context.Request.Path
            }
        };

        // Add trace ID for debugging
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}