namespace CloudWorld.Containers.Dapr.Producer.Features.SendMessage;

public record Message
{
    public required string Sender { get; init; }
    public required string Content { get; init; }
}