using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CloudWorld.Functions
{
    public class ServiceBusConsumer
    {
        private readonly ILogger<ServiceBusConsumer> _logger;

        public ServiceBusConsumer(ILogger<ServiceBusConsumer> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ServiceBusConsumer))]
        public async Task Run(
            [ServiceBusTrigger("billing", "azure-function", 
                Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

             // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
