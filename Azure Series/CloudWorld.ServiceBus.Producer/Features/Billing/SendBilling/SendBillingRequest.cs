namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public class SendBillingRequest
{
    public int OrderId { get; init; }
    public int BillingId { get; init; }
    public DateTime BillingDate { get; init; }
}