using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Mappers;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Api.Responses;
using MoneyTracker.Api.Filters;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;

namespace MoneyTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("API controller for managing transactions.")]
    public class TransactionsController(
        ITransactionService svc, 
        IValidator<TransactionSaveDto> createValidator,
        IValidator<TransactionQueryDto> queryValidator) : ControllerBase
    {
        private readonly ITransactionService _svc = svc;
        private readonly IValidator<TransactionSaveDto> _createValidator = createValidator;
        private readonly IValidator<TransactionQueryDto> _queryValidator = queryValidator;

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new <see cref="Transaction"/> based on the data provided in the
        /// <see cref="TransactionSaveDto"/>.  
        /// 
        /// If the payload fails validation, a 400 response with an <see cref="ErrorResponse"/> 
        /// is returned.  
        /// 
        /// On success, the newly created transaction is returned with a 201 status code, along with a
        /// <c>Location</c> header pointing to the <see cref="GetById"/> endpoint for retrieving the resource.
        /// 
        /// Unexpected server errors result in a 500 response.
        /// </remarks>
        /// <param name="dto">
        /// The data required to create a new transaction, including amount, type, category, date, and description.
        /// </param>
        /// <response code="201">
        /// Returned when the transaction is successfully created.  
        /// The response includes the created <see cref="Transaction"/> and the <c>Location</c> header.
        /// </response>
        /// <response code="400">
        /// Returned when the request body is invalid, as validated by the <see cref="ValidationFilterAttribute{T}"/>.
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs.
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Transaction))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<TransactionSaveDto>))]
        public async Task<IActionResult> Create(TransactionSaveDto dto)
        {
            var t = await _svc.CreateAsync(dto.Amount, dto.Type, dto.CategoryId, dto.Date, dto.Description);
            return CreatedAtAction(nameof(GetById), new { id = t.Id }, t.ToDto());
        }
        
        /// <summary>
        /// Returns a paginated list of transactions with optional filters.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves a paginated collection of <see cref="Transaction"/> records,
        /// supporting optional filters such as type, category, and date range.  
        /// 
        /// Pagination parameters (<c>PageNumber</c> and <c>PageSize</c>) and filters are provided through
        /// the query string using the <see cref="TransactionQueryDto"/>.  
        /// 
        /// If the query parameters fail validation, a 400 response with an <see cref="ErrorResponse"/> 
        /// is returned.  
        /// Unexpected server errors result in a 500 response.
        /// </remarks>
        /// <param name="dto">
        /// The query parameters used for filtering and paginating the transaction list.
        /// </param>
        /// <response code="200">
        /// Returns a <see cref="PagedResponse{T}"/> containing the list of transactions and pagination metadata.
        /// </response>
        /// <response code="400">
        /// Returned when the query parameters are invalid, as validated by the <see cref="ValidationFilterAttribute{T}"/>.
        /// </response>
        /// <response code="500">
        /// Returned when an unexpected server error occurs.
        /// </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Transaction>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<TransactionQueryDto>))]
        public async Task<IActionResult> List([FromQuery] TransactionQueryDto dto)
        {
            var result = await _svc.ListAsync(dto);

            var mapped = result.Items.Select(t => t.ToDto()).ToList();

            return Ok(new PagedResponse<TransactionDto>(
                mapped, result.PageNumber, result.PageSize, result.TotalCount
            ));
        }

        /// <summary>
        /// Retrieves a single transaction by its unique identifier.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a specific <see cref="Transaction"/> using its GUID identifier.
        ///
        /// **Behavior:**
        /// - If the transaction exists, it is returned with a **200 OK** response.
        /// - If no transaction is found for the given <paramref name="id"/>, a **404 Not Found** response is returned.
        /// - Any unexpected error results in a **500 Internal Server Error** response.
        ///
        /// **Example request:**
        /// ```http
        /// GET /api/transactions/{id}
        /// ```
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the transaction to retrieve.</param>
        /// <response code="200">Returns the transaction that matches the given identifier.</response>
        /// <response code="404">Returned when no transaction exists with the specified <paramref name="id"/>.</response>
        /// <response code="500">Returned when an unexpected server error occurs.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Transaction))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var t = await _svc.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t.ToDto());
        }

        /// <summary>
        /// Updates an existing transaction by replacing all of its data.
        /// </summary>
        /// <remarks>
        /// This operation follows the REST <c>PUT</c> semantics, meaning that the entire
        /// transaction resource is replaced with the data provided in the request body.
        /// All fields in <paramref name="dto"/> must be supplied, as partial updates 
        /// are not supported through this endpoint (use <c>PATCH</c> for partial updates).
        ///
        /// If the specified transaction does not exist, a <c>404 Not Found</c> is returned.
        /// Any unexpected errors will result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the transaction to update.</param>
        /// <param name="dto">The full set of transaction fields used to replace the existing resource.</param>
        /// <returns>
        /// <c>NoContent</c> when the update succeeds,
        /// <c>NotFound</c> if the transaction ID does not exist, or
        /// <c>InternalServerError</c> in case of unhandled exceptions.
        /// </returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<TransactionSaveDto>))]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionSaveDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            return Ok(updated.ToDto());
        }

        /// <summary>
        /// Partially updates an existing transaction.
        /// </summary>
        /// <remarks>
        /// This operation follows the REST <c>PATCH</c> semantics, applying only the
        /// fields provided in <paramref name="dto"/> to the existing resource.
        /// 
        /// Fields omitted from the request will remain unchanged.
        /// 
        /// If the transaction does not exist, a <c>404 Not Found</c> is returned.
        /// Validation errors will produce a <c>400 Bad Request</c>, and unexpected
        /// errors will result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the transaction to update.</param>
        /// <param name="dto">The set of fields to partially update on the resource.</param>
        /// <returns>
        /// <c>NoContent</c> when the partial update succeeds,
        /// <c>NotFound</c> if the transaction is not found,
        /// <c>BadRequest</c> for validation issues, or
        /// <c>InternalServerError</c> for unexpected exceptions.
        /// </returns>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<TransactionPatchDto>))]
        public async Task<IActionResult> Patch(Guid id, [FromBody] TransactionPatchDto dto)
        {
            var updated = await _svc.PatchAsync(id, dto);
            return Ok(updated.ToDto());
        }

        /// <summary>
        /// Deletes an existing transaction by its unique identifier.
        /// </summary>
        /// <remarks>
        /// This endpoint permanently removes a transaction from the system.
        /// 
        /// **Behavior:**
        /// - If the transaction exists, it is deleted and the API returns **204 No Content**.
        /// - If no transaction with the specified ID exists, the API returns **404 Not Found**.
        /// 
        /// **Use case example:**
        /// ```http
        /// DELETE /api/transactions/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// ```
        /// </remarks>
        /// <param name="id">The GUID identifying the transaction to delete.</param>
        /// <returns>
        /// A **204 No Content** response if deletion succeeds,  
        /// or **404 Not Found** if the transaction does not exist.
        /// </returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exists = await _svc.GetByIdAsync(id);
            if (exists == null)
                return NotFound();

            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
