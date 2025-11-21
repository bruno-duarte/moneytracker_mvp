using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _svc;
        public CategoriesController(ICategoryService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var c = await _svc.CreateAsync(dto.Name, dto.MonthlyLimit);
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var c = await _svc.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryCreateDto dto)
        {
            try
            {
                await _svc.UpdateAsync(id, dto.Name, dto.MonthlyLimit);
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

    public record CategoryCreateDto(string Name, decimal MonthlyLimit);
}
