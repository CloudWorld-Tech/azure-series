using Microsoft.Extensions.Logging;

namespace CloudWorld.Kubernetes;

public class SalutationService(ILogger<SalutationService> logger) : ISalutationService
{
    public void SayHello()
    {
        logger.LogInformation("SomeService created");
    }
}