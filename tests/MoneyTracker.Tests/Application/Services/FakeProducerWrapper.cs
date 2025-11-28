using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MoneyTracker.Infrastructure.Kafka;

namespace MoneyTracker.Application.Services
{
  public class FakeProducerWrapper : ProducerWrapper
  {
      public FakeProducerWrapper()
        : base(new ConfigurationBuilder()
               .AddInMemoryCollection(new Dictionary<string, string>
               {
                   { "Kafka:BootstrapServers", "localhost:9092" }
               })
               .Build())
    { }
  }
}
