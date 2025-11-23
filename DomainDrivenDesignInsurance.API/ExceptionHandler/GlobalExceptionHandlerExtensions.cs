namespace DomainDrivenDesignInsurance.API.ExceptionHandler;

public static class GlobalExceptionHandlerExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(
        this IServiceCollection service)
    {
        service.AddExceptionHandler<BadRequestExceptionHandler>();
        service.AddExceptionHandler<NotFoundExceptionHandler>();
        service.AddExceptionHandler<InvalidPeriodPolicyExceptionHandler>();
        service.AddExceptionHandler<InvalidInsuredNameExceptionHandler>();
        service.AddExceptionHandler<PremiumCalculationViolationExceptionHandler>();
        service.AddExceptionHandler<EmptyCoverageExceptionHandler>();
        service.AddExceptionHandler<ForbiddenExceptionHandler>();
        service.AddExceptionHandler<HighClaimsRatioExceptionHandler>();
        service.AddExceptionHandler<GlobalExceptionHandler>();
        return service;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
