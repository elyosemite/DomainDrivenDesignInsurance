using DomainDrivenDesignInsurance.Application.Commands;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.Entities;
using DomainDrivenDesignInsurance.Domain.PolicyAggregate.ValueObjects;
using DomainDrivenDesignInsurance.Domain.ValueObject;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API;

public static class PolicyApi
{
    public static WebApplication MapPolicyEndpointMap(this WebApplication app)
    {
        app.MapGet(
            "/v1/api/policy",
            async () =>
            {
                return Results.Ok("Domain Driven Design Insurance API is running...");
            }
        ).WithName("Policys")
         .WithTags("Policy");

        app.MapPost(
            "/v1/api/policy",
            async ([FromBody] PolicyDto policy, IMediator mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    var response = await mediator.Send(new IssuePolicyCommandRequest
                    {
                        PolicyId = policy.PolicyId,
                        PolicyHolderName = policy.PolicyHolderName,
                        StartDate = policy.Startdate,
                        EndDate = policy.EndDate,
                        Value = policy.Premium
                    }, cancellationToken);

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            }
        )
        .WithName("Policy")
        .WithTags("Policy");

         return app;
    }  
}

public sealed record PolicyDto(
    Guid PolicyId,
    string PolicyHolderName,
    DateTime Startdate,
    DateTime EndDate,
    decimal Premium);

public static class PolicyMapper
{
    public static Policy ToDomain(this PolicyDto dto)
    {
        var period = new Period(dto.Startdate, dto.EndDate);
        var placeHolderName = dto.PolicyHolderName;
        var brokerId = Guid.NewGuid();
        var insuredId = Guid.NewGuid();
        var coverages = new List<Coverage>
        {
            new Coverage("COV001", "Fire Coverage", new Money(100000, "USD"), 0.05m),
            new Coverage("COV002", "Theft Coverage", new Money(50000, "USD"), 0.03m)
        };
        
        return Policy.Issue(
            dto.PolicyId == Guid.Empty ? Guid.NewGuid() : dto.PolicyId,
            insuredId,
            placeHolderName,
            brokerId,
            period,
            coverages
        );
    }

    public static PolicyDto ToPolicyDto(this Policy dto)
    {
        return new PolicyDto(
            dto.InsuredId,
            dto.PolicyHolderName,
            dto.Period.From,
            dto.Period.To,
            dto.CalculateTotalPremium().Amount
        );
    }
}