using DomainDrivenDesignInsurance.Domain.Exceptions;

namespace DomainDrivenDesignInsurance.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        } 
        catch(Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            await HandleExceptionAsync(context, exception);
        }
    }

    public sealed record ErrorDetails(int StatusCode, string Message, Dictionary<string, string[]>? Errors);

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationEx => new ErrorDetails(StatusCodes.Status400BadRequest, "Validation failed", validationEx.Errors),
            NotFoundException notFoundEx => new ErrorDetails(StatusCodes.Status404NotFound, "Validation failed", notFoundEx.Errors),            
            UnauthorizedException unauthorizedException => new ErrorDetails(StatusCodes.Status401Unauthorized, "Unauthorized access", unauthorizedException.Errors),
            ForbiddenException forbiddenException => new ErrorDetails(StatusCodes.Status403Forbidden, "Access forbidden", forbiddenException.Errors),
            _ => new ErrorDetails(StatusCodes.Status500InternalServerError, "An internal server error occurred", null)
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
