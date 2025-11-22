using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Transactions
{
  public class TransactionPatchDto
  {
      public decimal? Amount { get; set; }
      public TransactionType? Type { get; set; }
      public Guid? CategoryId { get; set; }
      public DateTime? Date { get; set; }
      public string? Description { get; set; }
  }
}