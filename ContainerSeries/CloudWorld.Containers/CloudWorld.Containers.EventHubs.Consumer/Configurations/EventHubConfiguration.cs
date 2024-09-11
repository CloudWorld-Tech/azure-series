namespace CloudWorld.Containers.EventHubs.Consumer.Configurations;

public record EventHubConfiguration
{
    public string? NameSpace { get; set; }
    public string? EventHubName { get; set; }
    public string? ConsumerGroup { get; set; }
}