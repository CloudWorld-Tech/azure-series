namespace CloudWorld.ServiceBus.Consumer.Features.Orders.ProcessOrder;

public record ProcessOrderRequest
{
    public int OrderId { get; init; }
    public string OrderName { get; init; }
    public DateTime OrderDate { get; init; }
}