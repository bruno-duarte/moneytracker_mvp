using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using System;

namespace MoneyTracker.EventsService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _topic = "transaction.created";
        private readonly string _bootstrap = "localhost:9092";
        public Worker(ILogger<Worker> logger) => _logger = logger;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
        }

        private void ConsumeLoop(CancellationToken ct)
        {
            var config = new ConsumerConfig
            {
                GroupId = "afinpe-events-consumer",
                BootstrapServers = _bootstrap,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var c = new ConsumerBuilder<Ignore, string>(config).Build();
            c.Subscribe(_topic);
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    var cr = c.Consume(ct);
                    _logger.LogInformation("Received event from topic {Topic}: {Value}", cr.Topic, cr.Message.Value);
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                c.Close();
            }
        }
    }
}
