using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.Functions;

public class SignalRNegotiate(ILogger<SignalRNegotiate> logger)
{
    [Function("Negotiate")]
    public IActionResult Negotiate([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
        [SignalRConnectionInfoInput(HubName = "images", ConnectionStringSetting = "AzureSignalRConnectionString")]
        string connectionInfo)
    {
        return new OkObjectResult(connectionInfo);
    }
}