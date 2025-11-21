namespace MoneyTracker.Domain.Events
{
    public class TransactionCreatedEvent
    {
        public Guid TransactionId { get; }
        public decimal Amount { get; }
        public string Type { get; }
        public Guid CategoryId { get; }
        public DateTime CreatedAt { get; }

        public TransactionCreatedEvent(
            Guid transactionId,
            decimal amount,
            string type,
            Guid categoryId,
            DateTime createdAt)
        {
            TransactionId = transactionId;
            Amount = amount;
            Type = type;
            CategoryId = categoryId;
            CreatedAt = createdAt;
        }
    }
}
