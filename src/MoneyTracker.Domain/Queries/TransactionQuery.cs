using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Domain.Queries
{
    public class TransactionQuery
    {
        public TransactionType? Type { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
