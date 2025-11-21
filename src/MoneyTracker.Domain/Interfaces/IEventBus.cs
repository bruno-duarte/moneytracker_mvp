using MoneyTracker.Domain.Events;

public interface IEventBus
{
    Task PublishTransactionCreatedAsync(TransactionCreatedEvent evt);
}
