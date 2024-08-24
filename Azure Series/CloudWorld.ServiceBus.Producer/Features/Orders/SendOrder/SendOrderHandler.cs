using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Azure;

namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public class SendOrderHandler(IAzureClientFactory<ServiceBusClient> azureClientFactory)
    : IRequestHandler<SendOrderCommand, SendOrderResponse>
{
    public async Task<SendOrderResponse> Handle(SendOrderCommand request, CancellationToken cancellationToken)
    {
        var serviceBusClient = azureClientFactory.CreateClient("azure-labs-service-bus");
        var sender = serviceBusClient.CreateSender("orders");
        var message = new ServiceBusMessage(JsonSerializer.Serialize(request.Request));

        await sender.SendMessageAsync(message, cancellationToken);

        return new SendOrderResponse
        {
            Status = "Sent"
        };
    }
}