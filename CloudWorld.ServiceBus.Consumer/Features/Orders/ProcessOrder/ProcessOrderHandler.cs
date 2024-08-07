using MediatR;

namespace CloudWorld.ServiceBus.Consumer.Features.Orders.ProcessOrder;

public class ProcessOrderHandler: IRequestHandler<ProcessOrderCommand, ProcessOrderResponse>
{
    public async Task<ProcessOrderResponse> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
    {
        throw new Exception();
    }
}