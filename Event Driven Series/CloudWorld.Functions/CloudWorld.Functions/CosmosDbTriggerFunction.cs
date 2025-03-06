using CloudWorld.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CloudWorld.Functions;

public class CosmosDbTriggerFunction(ILogger<CosmosDbTriggerFunction> logger)
{
    [Function(nameof(CosmosDbTriggerFunction))]
    [SignalROutput(HubName = "images", ConnectionStringSetting = "AzureSignalRConnectionString")]
    public SignalRMessageAction Run([CosmosDBTrigger(
            "images-db",
            "images",
            Connection = "CosmosDBConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = false)]
        IReadOnlyList<ImageAnalysisDocument>? input)
    {
        return new SignalRMessageAction("imageProcessed")
        {
            Arguments = [input ?? []]
        };
    }
}