namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public class SendBillingResponse
{
    public string Status { get; set; }

    public DateTime EnqueuedTime { get; set; } = DateTime.UtcNow;
}