using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Mappers
{
    public static class TransactionPatchMapper
    {
        public static void MapChanges(this TransactionPatchDto dto, Transaction t)
        {
            t.Patch(
                dto.Amount,
                dto.Type,
                dto.CategoryId,
                dto.Date,
                dto.Description
            );
        }
    }
}
