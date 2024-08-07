using Azure.Identity;
using CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;
using CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder
        .AddServiceBusClientWithNamespace(builder.Configuration.GetConnectionString("ServiceBus"))
        .WithName("azure-labs-service-bus")
        .WithCredential(new DefaultAzureCredential());
});

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(SendOrderHandler).Assembly);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app
    .MapGroup("orders")
    .MapSendOrder();

app
    .MapGroup("billing")
    .MapSendBilling();

app.Run();