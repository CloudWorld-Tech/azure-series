using MediatR;

namespace CloudWorld.ServiceBus.Consumer.Features.Orders.ProcessOrder;

public record ProcessOrderCommand(ProcessOrderRequest Request) : IRequest<ProcessOrderResponse>;