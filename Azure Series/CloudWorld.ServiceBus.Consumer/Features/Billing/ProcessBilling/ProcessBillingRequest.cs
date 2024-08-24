namespace CloudWorld.ServiceBus.Consumer.Features.Billing.ProcessBilling;

public record ProcessBillingRequest
{
    public int OrderId { get; init; }
    public int BillingId { get; init; }
    public DateTime BillingDate { get; init; }
}