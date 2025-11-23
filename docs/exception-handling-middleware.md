
# Exception Handling Middleware

## Overview

This project includes custom Exception Handling Middlewares for ASP.NET Core applications. These middlewares provide centralized ways to handle exceptions and return consistent error responses in JSON or ProblemDetails format. They support domain-specific exceptions reflecting business rules for insurance policies.

## Features

- Handles domain exceptions and returns appropriate HTTP status codes.
- Returns error details in a structured JSON or ProblemDetails format.
- Logs unhandled exceptions for diagnostics.
- Supports extensibility for new domain exceptions and custom handlers.

## Existing Domain Exceptions

The following domain exceptions are implemented in the project:

- `DomainException` (base class)
- `ValidationException`
- `NotFoundException`
- `UnauthorizedException`
- `ForbiddenException`
- `BadRequestException`
- `HighClaimsRatioException`
- `PolicyOutOfValidityPeriodException`
- `PolicyAlreadyCancelledException`
- `PremiumCalculationViolationException`
- `InvalidCoverageException`
- `InvalidPeriodPolicyException`
- `EmptyCoverageException`
- `InvalidInsuredNameException`

These exceptions represent business rules and error conditions for insurance policies, such as invalid periods, forbidden actions, or premium calculation violations.

## Existing Exception Handlers

The following middlewares are available for exception handling in the API layer:

- `GlobalExceptionHandlingMiddleware`
- `ProblemDetailsMiddleware`
- `ProblemDetailGlobalExceptionHandlingMiddleware`
- `EnrichedExceptionHandlerMiddleware`

Each middleware provides a different approach for formatting and logging errors. You can choose the one that best fits your application's needs.

## Usage

### 1. Register the Middleware

Inject the desired middleware into the request pipeline in your `Program.cs` or `Startup.cs`:

```csharp
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
// or
app.UseProblemDetailsMiddleware();
// or
app.UseProblemDetailGlobalExceptionHandlingMiddleware();
// or
app.UseEnrichedExceptionHandlerMiddleware();
```

Register the middleware early in the pipeline, before other middlewares that may handle exceptions.

### 2. Exception Types Supported

The middlewares automatically handle the domain exceptions listed above, returning appropriate status codes and error details. Any other exception will result in a 500 Internal Server Error.

### 3. Error Response Format

All error responses are returned in JSON or ProblemDetails format, for example:

```json
{
  "StatusCode": 400,
  "Message": "Validation failed",
  "Errors": { "field": ["error message"] }
}
```

Or, using ProblemDetails:

```json
{
  "status": 404,
  "title": "Resource not found",
  "detail": "Policy with key '123' was not found",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "instance": "/v1/api/policy/123",
  "entityName": "Policy",
  "key": "123",
  "traceId": "..."
}
```

### 4. Logging

Unhandled exceptions are automatically logged using the built-in logger, including contextual information when using enriched handlers.

## How to Add a New Domain Exception and Handler

### 1. Create a New Domain Exception

Suppose you want to enforce a business rule: "A policy cannot be issued if the insured is under 18 years old." Create a new exception in the domain layer:

```csharp
// Domain/Exceptions/UnderageInsuredException.cs
namespace DomainDrivenDesignInsurance.Domain.Exceptions;

public class UnderageInsuredException : DomainException
{
  public UnderageInsuredException(string insuredName, int age)
    : base($"Insured '{insuredName}' is underage: {age} years old.")
  {
    SetErrors(new Dictionary<string, string[]>
    {
      { "InsuredName", new[] { insuredName } },
      { "Age", new[] { age.ToString() } }
    });
  }
}
```

### 2. Handle the New Exception in the API Layer

Extend your exception handling middleware to support the new exception. For example, in `GlobalExceptionHandlingMiddleware`:

```csharp
private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
  context.Response.ContentType = "application/json";

  var response = exception switch
  {
    UnderageInsuredException underageEx => new ErrorDetails(
      StatusCodes.Status400BadRequest,
      underageEx.Message,
      underageEx.Errors),
    // ... other cases
    _ => new ErrorDetails(StatusCodes.Status500InternalServerError, "An internal server error occurred", null)
  };

  context.Response.StatusCode = response.StatusCode;
  await context.Response.WriteAsJsonAsync(response);
}
```

If you use ProblemDetails, add a case for the new exception:

```csharp
private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
  var problemDetails = exception switch
  {
    UnderageInsuredException underageEx => new ProblemDetails
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Underage insured",
      Detail = underageEx.Message,
      Instance = context.Request.Path
    },
    // ... other cases
  };
  // ...
}
```

### 3. Throw the Exception in Domain Logic

Whenever the business rule is violated, throw the new exception in your domain service or aggregate:

```csharp
if (insured.Age < 18)
  throw new UnderageInsuredException(insured.Name, insured.Age);
```

## Notes

- Ensure your custom exceptions inherit from `DomainException` for consistent error handling.
- Update your middleware to handle new exceptions as needed.
- The middleware should be injected only once in the pipeline.
- Choose the middleware and response format that best fits your API design.
