using Azure.Messaging.ServiceBus;
using CloudWorld.ServiceBus.Consumer.Features.Billing.ProcessBilling;
using CloudWorld.ServiceBus.Consumer.Features.Orders.ProcessOrder;
using Microsoft.Extensions.Azure;

namespace CloudWorld.ServiceBus.Consumer;

public class Worker(ILogger<Worker> logger, IAzureClientFactory<ServiceBusClient> azureClientBuilder)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = azureClientBuilder.CreateClient("azure-labs-service-bus");
            var consumer = client.CreateReceiver("billing","background-job", new ServiceBusReceiverOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });
            
            var message = await consumer.ReceiveMessageAsync(cancellationToken: stoppingToken);
            
            if (message?.Body == null)
                continue;

            var content = message.Body.ToObjectFromJson<ProcessBillingRequest>();

            logger.LogInformation("Received message: {message}", content);
            await consumer.CompleteMessageAsync(message, stoppingToken);
        }
    }
}