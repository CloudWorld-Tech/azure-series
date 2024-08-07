using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public static class SendOrderEndpoint
{
    public static IEndpointRouteBuilder MapSendOrder(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/send",
            ([FromServices] IMediator mediator, SendOrderRequest request) =>
                mediator.Send(new SendOrderCommand(request)));

        return endpoints;
    }
}