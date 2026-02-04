using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Domain.Common;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Mappers;
using MoneyTracker.Domain.Specifications;
using MoneyTracker.Application.Exceptions;
using MoneyTracker.Messaging.Abstractions;
using MoneyTracker.Messaging.Abstractions.Models;
using MoneyTracker.Application.Messaging.Topics;
using MoneyTracker.Domain.Events;
using MoneyTracker.Application.DTOs.Reports;

namespace MoneyTracker.Application.Services
{
    public class TransactionService(
        ITransactionRepository repo, 
        ICategoryRepository categoryRepo, 
        IPersonRepository personRepo,
        IMessageProducer producer) : ITransactionService
    {
        public async Task<TransactionDto> CreateAsync(TransactionSaveDto dto)
        {
            var category = await categoryRepo.GetByIdAsync(dto.CategoryId)
                ?? throw new InvalidReferenceException("Category", dto.CategoryId);

            var person = await personRepo.GetByIdAsync(dto.PersonId)
                ?? throw new InvalidReferenceException("Person", dto.PersonId);

            var t = Transaction.Create(dto.Amount, dto.Type, category, person, dto.Date, dto.Description);

            await repo.AddAsync(t);

            await producer.PublishAsync(
                TransactionTopics.Created,
                new MessageEnvelope
                {
                    MessageType = nameof(TransactionCreatedEvent),
                    OccurredAt = DateTime.UtcNow,
                    Payload = t.ToEvent(),
                });

            return t.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id) => await repo.DeleteAsync(id);

        public async Task<TransactionDto?> GetByIdAsync(Guid id)
        {
            var t = await repo.GetByIdAsync(id);
            return t?.ToDto();
        }

        public async Task<PagedResult<TransactionDto>> ListAsync(TransactionQueryDto dto)
        {
            var query = dto.ToQuery();
            var spec = new TransactionFilterSpecification(query);
            var pagedResult = await repo.ListAsync(spec, query.PageNumber, query.PageSize);

            return new PagedResult<TransactionDto>(pagedResult.Items.Select(x => x.ToDto()), pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalCount);
        }

        public async Task<TransactionDto> UpdateAsync(Guid id, TransactionSaveDto dto)
        {
            var t = await repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            t.Update(dto.Amount, dto.Type, dto.CategoryId, dto.Date, dto.Description);
            await repo.UpdateAsync(t);

            return t.ToDto();
        }

        public async Task<TransactionDto> PatchAsync(Guid id, TransactionPatchDto dto)
        {
            var t = await repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            dto.MapChanges(t);

            await repo.UpdateAsync(t);

            return t.ToDto();
        }

		public Task ProcessCreatedTransactionEventAsync(TransactionCreatedEvent ev, CancellationToken cancellationToken)
		{
            // implement event processing logic here
			return Task.CompletedTask;
		}

        public async Task<List<PersonSummaryDto>> GetTotalsByPersonAsync()
        {
            var groups = await repo.GetTotalsByPersonAsync();
            return [.. groups
                .Select(g => new PersonSummaryDto
                {
                    PersonId = g.Key.PersonId,
                    Name = g.Key.Name,
                    TotalIncome = g.Where(t => t.Type == TransactionType.Income)
                                .Sum(t => t.Amount.Value),
                    TotalExpense = g.Where(t => t.Type == TransactionType.Expense)
                                    .Sum(t => t.Amount.Value)
                })];
        }
	}
}
