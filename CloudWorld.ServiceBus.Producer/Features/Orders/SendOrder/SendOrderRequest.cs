namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public record SendOrderRequest
{
    public int OrderId { get; init; }
    public string OrderName { get; init; }
    public DateTime OrderDate { get; init; }
}