using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.EventsService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
