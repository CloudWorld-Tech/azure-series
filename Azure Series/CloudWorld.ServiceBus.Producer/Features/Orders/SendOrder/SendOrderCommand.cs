using MediatR;

namespace CloudWorld.ServiceBus.Producer.Features.Orders.SendOrder;

public record SendOrderCommand(SendOrderRequest Request) : IRequest<SendOrderResponse>;