using DomainDrivenDesignInsurance.Domain.Exceptions;

namespace DomainDrivenDesignInsurance.API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            PremiumCalculationViolationException premiumEx => new ErrorDetails(StatusCodes.Status400BadRequest, "Premium calculation error", premiumEx.Errors),
            ValidationException validationEx => new ErrorDetails(StatusCodes.Status400BadRequest, "Validation failed", validationEx.Errors),
            NotFoundException notFoundEx => new ErrorDetails(StatusCodes.Status404NotFound, "Validation failed", notFoundEx.Errors),            
            UnauthorizedException unauthorizedException => new ErrorDetails(StatusCodes.Status401Unauthorized, "Unauthorized access", unauthorizedException.Errors),
            ForbiddenException forbiddenException => new ErrorDetails(StatusCodes.Status403Forbidden, "Access forbidden", forbiddenException.Errors),
            InvalidPeriodPolicyException invalidPeriodEx => new ErrorDetails(StatusCodes.Status400BadRequest, invalidPeriodEx.Message, invalidPeriodEx.Errors),
            _ => new ErrorDetails(StatusCodes.Status500InternalServerError, "An internal server error occurred", null)
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}

public static class GlobalExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}