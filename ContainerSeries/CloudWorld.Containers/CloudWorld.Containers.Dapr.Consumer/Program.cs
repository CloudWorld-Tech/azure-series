using CloudWorld.Containers.Dapr.Producer.Features.SendMessage;
using Dapr;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapPost("/messages", [Topic("pubsub", "messages")](ILogger<Program> logger, Message item) =>
{
    Console.WriteLine($"Message Received :{item.Sender}: {item.Content}");
    return Results.Ok();
});

app.Run();