using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Transactions
{
  public record TransactionSaveDto(decimal Amount, TransactionType Type, Guid CategoryId, DateTime Date, string? Description);
}
