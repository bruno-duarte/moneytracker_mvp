using MoneyTracker.Domain.Events;

namespace MoneyTracker.Infrastructure.Kafka
{
  public class KafkaEventBus : IEventBus
  {
    private readonly ProducerWrapper _producer;
    public KafkaEventBus(ProducerWrapper producer) { _producer = producer; }

    public Task PublishTransactionCreatedAsync(TransactionCreatedEvent evt)
        => _producer.ProduceAsync("transaction.created", evt);
  }
}

