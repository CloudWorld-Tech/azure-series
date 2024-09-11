using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using MediatR;
using Microsoft.Extensions.Azure;

namespace CloudWorld.Containers.EventHubs.Producer.Features.SendMessage;

public class SendMessageHandler(
    ILogger<SendMessageHandler> logger,
    IAzureClientFactory<EventHubProducerClient> azureClientFactory) : IRequestHandler<SendMessageCommand>
{
    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var eventHubProducerClient = azureClientFactory.CreateClient("EventHubProducerClient");
        
        var eventBatch =
            await eventHubProducerClient.CreateBatchAsync(
                new CreateBatchOptions { PartitionId = request.MessageRequest.PartitionId },
                cancellationToken);

        if (request.MessageRequest.Message == null)
            throw new ArgumentNullException(nameof(request.MessageRequest.Message));

        if (eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(request.MessageRequest.Message))))
            logger.LogInformation("Message added to batch");
        else
            logger.LogError("Message could not be added to batch");

        await eventHubProducerClient.SendAsync(eventBatch, cancellationToken);
    }
}