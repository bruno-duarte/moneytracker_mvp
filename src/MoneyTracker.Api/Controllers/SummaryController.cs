using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Domain.Enums;
using System.Linq;

namespace MoneyTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController(ITransactionService transactions) : ControllerBase
    {
        private readonly ITransactionService _transactions = transactions;

        [HttpGet("monthly")]
        public async Task<IActionResult> Monthly([FromQuery] int year, [FromQuery] int month)
        {
            var txs = (await _transactions.ListByMonthAsync(year, month)).ToList();

            var income = txs.Where(t => t.Type == Domain.Enums.TransactionType.Income).Sum(t => t.Amount);
            var expenses = txs.Where(t => t.Type == Domain.Enums.TransactionType.Expense).Sum(t => t.Amount);

            var categoryTotals = txs.GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            return Ok(new {
                Year = year,
                Month = month,
                MonthlyIncome = income,
                MonthlyExpenses = expenses,
                CategoryTotals = categoryTotals
            });
        }
    }
}
