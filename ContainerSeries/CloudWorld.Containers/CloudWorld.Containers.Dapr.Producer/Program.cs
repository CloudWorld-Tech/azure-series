using Dapr.Client;

for (var i = 1; i <= 10; i++)
{
    var order = new Message
    {
        Sender = "Producer",
        Content = $"Order {i}"
    };

    using var client = new DaprClientBuilder().Build();

    Console.WriteLine($"Sending message: {order.Content}");

    await client.PublishEventAsync("pubsub", "messages", order);
    await Task.Delay(TimeSpan.FromSeconds(1));
}

public record Message
{
    public required string Sender { get; init; }
    public required string Content { get; init; }
}