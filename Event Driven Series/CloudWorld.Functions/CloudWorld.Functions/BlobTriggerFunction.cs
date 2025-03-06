using CloudWorld.Functions.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace CloudWorld.Functions;

public class BlobTriggerFunction(
    ILogger<BlobTriggerFunction> logger,
    IAzureClientFactory<ComputerVisionClient> azureClientFactory)
{
    [Function(nameof(BlobTriggerFunction))]
    [CosmosDBOutput("images-db", "images", Connection = "CosmosDBConnection", CreateIfNotExists = false)]
    public async Task<ImageAnalysisDocument> Run(
        [BlobTrigger("attachments/{name}", Connection = "StorageAccount")]
        Stream stream,
        string name,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("C# Blob trigger function Processed blob Name: {name}", name);

        var client = azureClientFactory.CreateClient("ComputerVision");

        var features = await client.AnalyzeImageInStreamAsync(stream,
            new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Objects,
                VisualFeatureTypes.Description
            },
            cancellationToken: cancellationToken);

        var tags = features.Tags.Select(t => t.Name).ToList();
        var objects = features.Objects.Select(o => o.ObjectProperty).ToList();
        var description = features.Description.Captions.Select(c => c.Text).ToList();

        return new ImageAnalysisDocument
        {
            Tags = tags,
            Objects = objects,
            Description = description,
            ImageId = name,
            Id = Guid.NewGuid().ToString()
        };
    }
}