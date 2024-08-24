namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public class SendOrderResponse
{
    public string Status { get; set; }
    public DateTimeOffset EnqueuedTime { get; set; } = DateTimeOffset.UtcNow;
}