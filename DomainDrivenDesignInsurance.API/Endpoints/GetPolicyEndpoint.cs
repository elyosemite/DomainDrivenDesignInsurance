using DomainDrivenDesignInsurance.Application.Queries;
using FastEndpoints;
using Mediator;

namespace DomainDrivenDesignInsurance.API.Endpoints;

public sealed record GetPolicyByIdRequest
{
    [QueryParam]
    public Guid PolicyId { get; set; }
}

public sealed record GetPolicyByIdResponse(Guid PolicyId, string PolicyHolderName, decimal Premium);

public class GetPolicyByIdQueryEndpoint : Endpoint<GetPolicyByIdRequest, GetPolicyByIdResponse>
{
    private readonly IMediator _mediator;

    public GetPolicyByIdQueryEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/v1/api/policy");
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

            var result = new GetPolicyByIdResponse(
                response.PolicyId,
                response.PolicyHolderName,
                response.TotalPremiumAmount);

            await Send.OkAsync(result, ct);
        }
        catch (Exception)
        {
            await Send.NotFoundAsync(ct);
            throw;
        }
    }
}