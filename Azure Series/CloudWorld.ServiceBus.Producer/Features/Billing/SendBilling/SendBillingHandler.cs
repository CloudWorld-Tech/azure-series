using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Azure;

namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public class SendBillingHandler(IAzureClientFactory<ServiceBusClient> azureClientFactory)
    : IRequestHandler<SendBillingCommand, SendBillingResponse>
{
    public async Task<SendBillingResponse> Handle(SendBillingCommand request, CancellationToken cancellationToken)
    {
        var serviceBusClient = azureClientFactory.CreateClient("azure-labs-service-bus");
        var sender = serviceBusClient.CreateSender("billing");
        var message = new ServiceBusMessage(JsonSerializer.Serialize(request.Request));

        await sender.SendMessageAsync(message, cancellationToken);

        return new SendBillingResponse
        {
            Status = "Sent"
        };
    }
}