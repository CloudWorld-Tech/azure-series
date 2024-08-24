using MediatR;

namespace CloudWorld.ServiceBus.Producer.Features.Billing.SendBilling;

public record SendBillingCommand(SendBillingRequest Request) : IRequest<SendBillingResponse>;