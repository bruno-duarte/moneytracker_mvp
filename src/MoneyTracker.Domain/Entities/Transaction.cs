using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }

        public Guid CategoryId { get; private set; }

        public Category? Category { get; private set; }

        public DateTime Date { get; private set; }
        public string? Description { get; private set; }

        #pragma warning disable CS8618
        private Transaction() { }
        #pragma warning restore CS8618

        public Transaction(
            Guid id,
            decimal amount,
            TransactionType type,
            Guid categoryId,
            DateTime date,
            string? description)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            if (categoryId == Guid.Empty)
                throw new ArgumentException("CategoryId is required", nameof(categoryId));

            Amount = amount;
            Type = type;
            CategoryId = categoryId;
            Date = date;
            Description = description;
        }

        public void Update(
            decimal amount,
            TransactionType type,
            Guid categoryId,
            DateTime date,
            string? description)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));

            if (categoryId == Guid.Empty)
                throw new ArgumentException("CategoryId is required", nameof(categoryId));

            Amount = amount;
            Type = type;
            CategoryId = categoryId;
            Date = date;
            Description = description;
        }

        public void Patch(
            decimal? amount,
            TransactionType? type,
            Guid? categoryId,
            DateTime? date,
            string? description)
        {
            if (amount.HasValue)
                Amount = amount.Value;

            if (type.HasValue)
                Type = type.Value;

            if (categoryId.HasValue)
                CategoryId = categoryId.Value;

            if (date.HasValue)
                Date = date.Value;

            if (description != null)
                Description = description;
        }
    }
}
