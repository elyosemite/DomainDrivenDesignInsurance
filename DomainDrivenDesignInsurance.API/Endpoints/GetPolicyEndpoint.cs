using DomainDrivenDesignInsurance.Application.Queries;
using FastEndpoints;
using Mediator;

namespace DomainDrivenDesignInsurance.API.Endpoints;

public sealed record GetPolicyByIdRequest
{
    [QueryParam]
    public Guid PolicyId { get; set; }
}

public class GetPolicyByIdQueryEndpoint : Endpoint<GetPolicyByIdRequest, GetPolicyByIdQueryResponse>
{
    private readonly IMediator _mediator;

    public GetPolicyByIdQueryEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/v1/api/policy");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPolicyByIdRequest req, CancellationToken ct)
    {
        try
        {
            var query = new GetPolicyByIdQueryRequest(req.PolicyId);
            var response = await _mediator.Send(query, ct);

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