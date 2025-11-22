using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Domain.Queries;

namespace MoneyTracker.Application.Mappers
{
  public static class TransactionQueryMapper
  {
      public static TransactionQuery ToQuery(this TransactionQueryDto dto)
      {
          return new TransactionQuery
          {
              Type = dto.Type,
              CategoryId = dto.CategoryId,
              From = dto.From,
              To = dto.To,
              PageNumber = dto.PageNumber,
              PageSize = dto.PageSize
          };
      }
  }
}