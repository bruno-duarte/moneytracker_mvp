using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController(ITransactionService svc) : ControllerBase
    {
        private readonly ITransactionService _svc = svc;

        /// <summary>
        /// Create a new transaction.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionCreateDto dto)
        {
            var t = await _svc.CreateAsync(dto.Amount, dto.Type, dto.Category, dto.Date, dto.Description);
            return CreatedAtAction(nameof(GetById), new { id = t.Id }, t);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var t = await _svc.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionCreateDto dto)
        {
            try
            {
                await _svc.UpdateAsync(id, dto.Amount, dto.Type, dto.Category, dto.Date, dto.Description);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }

    public record TransactionCreateDto(decimal Amount, TransactionType Type, string Category, DateTime Date, string? Description);
}
