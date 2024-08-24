using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public static class SendBillingEndpoint
{
    public static IEndpointRouteBuilder MapSendBilling(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/send", async ([FromServices] IMediator mediator,
                [FromBody] SendBillingRequest request,
                CancellationToken cancellationToken = default) =>
            await mediator.Send(new SendBillingCommand(request), cancellationToken));

        return endpoints;
    }
}