using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using DomainDrivenDesignInsurance.Application.Exceptions;

namespace DomainDrivenDesignInsurance.Tests.ExceptionsTests;

public class ExceptionHandlingMiddlewareTests
{
    [Test]
    public async Task Should_Return_NotFound_For_NotFoundException()
    {
        // Arrange
        var middleware = new GlobalExceptionHandlerMiddleware(
            context => throw new NotFoundException("User", 123),
            Mock.Of<ILogger<GlobalExceptionHandlerMiddleware>>());

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(404, context.Response.StatusCode);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ProblemDetails>(
            context.Response.Body);
        
        Assert.Equal("Resource not found", response.Title);
    }

    [Test]
    public async Task Should_Return_500_For_UnhandledException()
    {
        // Arrange
        var middleware = new GlobalExceptionHandlerMiddleware(
            context => throw new InvalidOperationException("Unexpected error"),
            Mock.Of<ILogger<GlobalExceptionHandlerMiddleware>>());

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(500, context.Response.StatusCode);
    }
}