using DomainDrivenDesignInsurance.Application.Commands;
using FastEndpoints;
using Mediator;

namespace DomainDrivenDesignInsurance.API.Endpoints;

public class IssuePolicyEndpoint : Endpoint<IssuePolicyCommandRequest, IssuePolicyCommandResponse>
{
    private readonly IMediator _mediator;

    public IssuePolicyEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    public override void Configure()
    {
        Post("/v1/api/policy");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IssuePolicyCommandRequest req, CancellationToken ct)
    {
        try
        {
            var response = await _mediator.Send(req, ct);

            if (response == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }
            await Send.OkAsync(response, ct);
        }
        catch (Exception)
        {
            await Send.NotFoundAsync(ct);
            throw;
        }
    }
}