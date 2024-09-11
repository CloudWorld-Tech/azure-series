using Azure.Identity;
using CloudWorld.Containers.EventHubs.Producer.Configurations;
using CloudWorld.Containers.EventHubs.Producer.Features.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var eventHubConfiguration = builder.Configuration.GetSection("EventHubConfiguration").Get<EventHubConfiguration>();

if(eventHubConfiguration == null)
{
    throw new ArgumentNullException(nameof(eventHubConfiguration));
}

builder.Services.AddAzureClients(factoryBuilder =>
{
    factoryBuilder.AddEventHubProducerClientWithNamespace(eventHubConfiguration.NameSpace, eventHubConfiguration.EventHubName)
        .WithName("EventHubProducerClient")
        .WithCredential(new DefaultAzureCredential());
});

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(SendMessageCommand).Assembly);
});

var app = builder.Build();

app.MapPost("/api/eventhub", async ([FromBody] SendMessageRequest request, IMediator mediator) =>
{
    await mediator.Send(new SendMessageCommand(request));
    return Results.Ok();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();