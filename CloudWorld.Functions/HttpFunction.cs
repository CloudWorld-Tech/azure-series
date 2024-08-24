using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudWorld.Functions
{
    public class HttpFunction(ILogger<HttpFunction> logger)
    {
        [Function("HttpFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Admin,
            "get", "post")] HttpRequest req)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
