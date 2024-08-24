using CloudWorld.Kubernetes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<ISalutationService, SalutationService>();
using var host = builder.Build();

var salutationService = host.Services.GetRequiredService<ISalutationService>();
salutationService.SayHello();

await host.RunAsync();