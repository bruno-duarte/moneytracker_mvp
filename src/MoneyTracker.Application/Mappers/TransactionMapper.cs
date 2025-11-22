using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Mappers
{
  public static class TransactionMapper
  {
      public static TransactionDto ToDto(this Transaction t)
      {
        return new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type,
            CategoryId = t.CategoryId,
            Date = t.Date,
            Description = t.Description
        };
      }
  }
}