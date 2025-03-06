using Azure.Core;
using Azure.Identity;
using CloudWorld.Functions.Options;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Rest;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder.AddBlobServiceClient(builder.Configuration.GetValue<Uri>("BlobStorageUri"))
        .WithName("BlobStorage")
        .WithCredential(new DefaultAzureCredential());

    factoryBuilder.AddClient<ComputerVisionClient, ComputerVisionClientOptions>(
            (_, tokenCredential, _) =>
            {
                var token = tokenCredential.GetToken(
                    new TokenRequestContext(["https://cognitiveservices.azure.com/.default"]),
                    CancellationToken.None);

                return new ComputerVisionClient(
                    new TokenCredentials(token.Token))
                {
                    Endpoint = builder.Configuration.GetValue<string>("ComputerVisionEndpoint")
                };
            })
        .WithName("ComputerVision")
        .WithCredential(new DefaultAzureCredential());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AzureFunctionsOrigins",
        policy  =>
        {
            policy.WithOrigins("http://localhost:7071")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Build().Run();