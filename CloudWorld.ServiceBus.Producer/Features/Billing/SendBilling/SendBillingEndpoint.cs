using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public static class SendBillingEndpoint
{
    public static IEndpointRouteBuilder MapSendBilling(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/send",
            ([FromServices] IMediator mediator, SendBillingRequest request) =>
                mediator.Send(new SendBillingCommand(request)));

        return endpoints;
    }
}