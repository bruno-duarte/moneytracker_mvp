using MoneyTracker.Domain.Events;

namespace MoneyTracker.Infrastructure.Kafka
{
  public class KafkaEventBus(ProducerWrapper producer) : IEventBus
  {
    private readonly ProducerWrapper _producer = producer;

    public Task PublishTransactionCreatedAsync(TransactionCreatedEvent evt)
        => _producer.ProduceAsync("transaction.created", evt);
  }
}

