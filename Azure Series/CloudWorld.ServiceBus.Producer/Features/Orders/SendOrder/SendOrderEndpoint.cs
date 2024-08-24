using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public static class SendOrderEndpoint
{
    public static IEndpointRouteBuilder MapSendOrder(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/send", async ([FromServices] IMediator mediator,
                [FromBody] SendOrderRequest request,
                CancellationToken cancellationToken = default) =>
            await mediator.Send(new SendOrderCommand(request), cancellationToken));

        return endpoints;
    }
}