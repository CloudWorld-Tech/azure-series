namespace CloudWorld.Containers.EventHubs.Producer.Features.SendMessage;

public record SendMessageRequest
{
    public string? Message { get; set; }
    public string? Messenger { get; set; }
    public DateTime SentAt { get; set; }
    public string? PartitionId { get; set; }
}