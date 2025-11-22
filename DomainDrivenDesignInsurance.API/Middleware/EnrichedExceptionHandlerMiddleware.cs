using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace DomainDrivenDesignInsurance.API.Middleware;

public class EnrichedExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EnrichedExceptionHandlerMiddleware> _logger;

    public EnrichedExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<EnrichedExceptionHandlerMiddleware> logger)
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
        catch (Exception ex)
        {
            await LogExceptionWithContextAsync(context, ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task LogExceptionWithContextAsync(HttpContext context, Exception exception)
    {
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var requestBody = await GetRequestBodyAsync(context);

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["TraceId"] = context.TraceIdentifier,
            ["UserId"] = userId ?? "Anonymous",
            ["RequestPath"] = context.Request.Path,
            ["RequestMethod"] = context.Request.Method,
            ["QueryString"] = context.Request.QueryString.ToString(),
            ["RequestBody"] = requestBody,
            ["UserAgent"] = context.Request.Headers["User-Agent"].ToString(),
            ["IpAddress"] = context.Connection.RemoteIpAddress?.ToString()
        }))
        {
            _logger.LogError(exception, 
                "Unhandled exception occurred for {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);
        }
    }

    private static async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        
        using var reader = new StreamReader(
            context.Request.Body,
            Encoding.UTF8,
            leaveOpen: true);
        
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        
        // Sanitize sensitive data
        return SanitizeSensitiveData(body);
    }

    private static string SanitizeSensitiveData(string data)
    {
        // Remove passwords, tokens, credit cards, etc.
        var patterns = new[]
        {
            @"""password""\s*:\s*""[^""]*""",
            @"""token""\s*:\s*""[^""]*""",
            @"""creditCard""\s*:\s*""[^""]*"""
        };

        foreach (var pattern in patterns)
        {
            data = Regex.Replace(data, pattern, "\"***REDACTED***\"", RegexOptions.IgnoreCase);
        }

        return data;
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Handle exception response
    }
}