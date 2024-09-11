using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Azure;

namespace CloudWorld.Containers.EventHubs.Consumer;

public class Worker(ILogger<Worker> logger, IAzureClientFactory<EventHubConsumerClient> azureClientFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventHubConsumerClient = azureClientFactory.CreateClient("EventHubConsumerClient");
        const string partitionId = "0";

        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var partitionEvent in eventHubConsumerClient.ReadEventsFromPartitionAsync(
                               partitionId,
                               EventPosition.Latest,
                               stoppingToken))
            {
                logger.LogInformation("Event received on partition {partitionId}: {data}",
                    partitionEvent.Partition.PartitionId, Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}