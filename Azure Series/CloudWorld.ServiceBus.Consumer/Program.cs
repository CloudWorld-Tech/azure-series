using Azure.Identity;
using CloudWorld.ServiceBus.Consumer;
using Microsoft.Extensions.Azure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder
        .AddServiceBusClientWithNamespace(builder.Configuration.GetConnectionString("ServiceBus"))
        .WithName("azure-labs-service-bus")
        .WithCredential(new DefaultAzureCredential());
});

var host = builder.Build();
host.Run();