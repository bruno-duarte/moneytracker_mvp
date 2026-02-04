using MoneyTracker.Domain.Common;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Domain.Events;

namespace MoneyTracker.Application.Services.Interfaces
{
  public interface ITransactionService
  {
    Task<TransactionDto> CreateAsync(TransactionSaveDto dto);
    Task<TransactionDto> UpdateAsync(Guid id, TransactionSaveDto dto);
    Task<TransactionDto> PatchAsync(Guid id, TransactionPatchDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<PagedResult<TransactionDto>> ListAsync(TransactionQueryDto dto);
    Task ProcessCreatedTransactionEventAsync(TransactionCreatedEvent ev, CancellationToken cancellationToken);
  }
}