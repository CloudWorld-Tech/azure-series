using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace CloudWorld.Functions;

public class UploadBlobFunction(
    ILogger<UploadBlobFunction> logger,
    IAzureClientFactory<BlobServiceClient> blobServiceClientBuilder)
{
    [Function(nameof(UploadBlobFunction))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var file = request.Form.Files["file"];
        var containerName = request.Query["container"];

        if (file == null || containerName.Count == 0)
            return new BadRequestObjectResult("Please provide a file and container name.");

        var (blobServiceClient, blobClient) = await UploadBlob(containerName, file, cancellationToken);
        var blobUri = await GenerateSasUri(blobServiceClient, blobClient, cancellationToken);
        return new OkObjectResult(blobUri);
    }

    async private static Task<IActionResult> GenerateSasUri(BlobServiceClient blobServiceClient,
        BlobClient blobClient,
        CancellationToken cancellationToken = default)
    {
        var userDelegationKey =
            await blobServiceClient.GetUserDelegationKeyAsync(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddMinutes(10), cancellationToken);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

        var uriBuilder = new BlobUriBuilder(blobClient.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(
                userDelegationKey,
                blobServiceClient.AccountName)
        };

        return new OkObjectResult(uriBuilder.ToUri());
    }

    async private Task<(BlobServiceClient blobServiceClient, BlobClient blobClient)> UploadBlob(
        StringValues containerName,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var blobServiceClient = blobServiceClientBuilder.CreateClient("BlobStorage");
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(file.FileName);
        await blobClient.UploadAsync(file.OpenReadStream(), true, cancellationToken);
        return (blobServiceClient, blobClient);
    }
}