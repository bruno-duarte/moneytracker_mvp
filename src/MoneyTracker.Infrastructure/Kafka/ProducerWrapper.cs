using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using System;

namespace MoneyTracker.Infrastructure.Kafka
{
    public class ProducerWrapper : IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public ProducerWrapper(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            var servers = config.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
            _topic = config.GetValue<string>("Kafka:Topic_TransactionCreated") ?? "transaction.created";
            var cfg = new ProducerConfig { BootstrapServers = servers };
            _producer = new ProducerBuilder<Null, string>(cfg).Build();
        }

        public Task ProduceAsync(string topic, object payload)
        {
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = false });
            return _producer.ProduceAsync(topic ?? _topic, new Message<Null, string> { Value = json });
        }

        public void Dispose() => _producer.Flush(TimeSpan.FromSeconds(5));
    }
}
