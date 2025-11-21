using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Infrastructure.Kafka;
using MoneyTracker.Application.Services.Interfaces;

namespace MoneyTracker.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly ProducerWrapper _producer;

        public TransactionService(ITransactionRepository repo, ProducerWrapper producer)
        {
            _repo = repo;
            _producer = producer;
        }

        public async Task<Transaction> CreateAsync(decimal amount, TransactionType type, string category, DateTime date, string? desc)
        {
            var t = new Transaction(Guid.NewGuid(), amount, type, category, date, desc);
            await _repo.AddAsync(t);
            // publish event
            await _producer.ProduceAsync("transaction.created", t);
            return t;
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<Transaction?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Transaction>> ListAsync() => await _repo.ListAsync();

        public async Task<IEnumerable<Transaction>> ListByMonthAsync(int year, int month) => await _repo.ListByMonthAsync(year, month);

        public async Task UpdateAsync(Guid id, decimal amount, TransactionType type, string category, DateTime date, string? desc)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) throw new Exception("Not found");
            t.Update(amount, type, category, date, desc);
            await _repo.UpdateAsync(t);
        }
    }
}
