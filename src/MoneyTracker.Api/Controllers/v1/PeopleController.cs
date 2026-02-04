using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Api.Filters;
using MoneyTracker.Api.Responses;
using MoneyTracker.Application.DTOs.People;
using MoneyTracker.Application.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MoneyTracker.Api.Controllers.v1
{
    /// <summary>
    /// API controller for managing people used in transactions.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for creating, listing, retrieving, updating,
    /// and deleting people.
    /// </remarks>
	[ApiController]
	[Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("API controller for managing people.")]
	 public class PeopleController(IPersonService svc) : ControllerBase
    {
        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new person with the provided name and age.
        ///
        /// On success, it returns a <c>201 Created</c> response along with
        /// the created person and a <c>Location</c> header pointing to
        /// the <see cref="GetById"/> endpoint.
        /// </remarks>
        /// <param name="dto">The data required to create the person.</param>
        /// <response code="201">The person was successfully created.</response>
        /// <response code="400">Returned when the request body is invalid.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PersonDto))]
        public async Task<IActionResult> Create([FromBody] PersonSaveDto dto)
        {
            var p = await svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        /// <summary>
        /// Retrieves a paginated list of categories.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a paginated collection of person records, optionally
        /// filtered based on the properties provided through the query string using
        /// <see cref="PersonQueryDto"/>.
        ///
        /// Pagination is controlled using the <c>PageNumber</c> and <c>PageSize</c>
        /// query parameters.
        ///
        /// The response includes pagination metadata via HTTP headers:
        /// <c>X-Pagination-TotalCount</c>, <c>X-Pagination-PageNumber</c>,
        /// and <c>X-Pagination-PageSize</c>.
        ///
        /// If the query parameters fail validation, a <c>400 Bad Request</c> is returned
        /// with an <see cref="ErrorResponse"/>.
        /// Unexpected server errors will result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="query">The query parameters used for filtering and pagination.</param>
        /// <returns>
        /// A paginated list of people, or an error response in case of validation
        /// issues or unhandled exceptions.
        /// </returns>
        /// <response code="200">Returns the filtered and paginated list of people.</response>
        /// <response code="400">Validation errors in the query parameters.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonDto>))]
        public async Task<IActionResult> List([FromQuery] PersonQueryDto query)
        {
            var result = await svc.ListAsync(query);

            var items = result.Items.ToList();

            Response.Headers.Append("X-Pagination-TotalCount", result.TotalCount.ToString());
            Response.Headers.Append("X-Pagination-PageNumber", result.PageNumber.ToString());
            Response.Headers.Append("X-Pagination-PageSize", result.PageSize.ToString());

            return Ok(items);
        }

        /// <summary>
        /// Retrieves a person by its unique identifier.
        /// </summary>
        /// <remarks>
        /// If the person exists, it is returned with a <c>200 OK</c> response.  
        /// If not found, a <c>404 Not Found</c> is returned.
        /// </remarks>
        /// <param name="id">The unique GUID identifying the person.</param>
        /// <response code="200">Returns the matching person.</response>
        /// <response code="404">Returned when the person does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var p = await svc.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        /// <summary>
        /// Fully updates an existing person by replacing all of its data.
        /// </summary>
        /// <remarks>
        /// This operation follows REST <c>PUT</c> semantics, meaning that the entire
        /// person resource is replaced using the data provided in the request body.
        /// All fields in <paramref name="dto"/> must be supplied, as partial updates
        /// are not supported through this endpoint (use <c>PATCH</c> for partial updates).
        ///
        /// If the specified person does not exist, a <c>404 Not Found</c> response is returned.
        /// If validation fails, the request results in a <c>400 Bad Request</c>.
        ///
        /// On success, the endpoint returns <c>200 OK</c> along with the updated
        /// person data.  
        /// Unexpected server errors result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the person to update.</param>
        /// <param name="dto">The full set of person fields used to replace the existing resource.</param>
        /// <returns>
        /// <c>Ok</c> with the updated person when the replacement succeeds,  
        /// <c>NotFound</c> if the person is not found,  
        /// <c>BadRequest</c> for validation issues, or  
        /// <c>InternalServerError</c> for unexpected exceptions.
        /// </returns>
        /// <response code="200">The person was successfully updated and returned in the response body.</response>
        /// <response code="400">Returned when validation of the input data fails.</response>
        /// <response code="404">Returned when the person does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PersonSaveDto dto)
        {
            var updated = await svc.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Partially updates an existing person.
        /// </summary>
        /// <remarks>
        /// This operation follows REST <c>PATCH</c> semantics, applying only the
        /// fields provided in <paramref name="dto"/> to the existing resource.
        /// Fields omitted from the request body will remain unchanged.
        ///
        /// If the specified person does not exist, a <c>404 Not Found</c> response is returned.
        /// If validation fails, a <c>400 Bad Request</c> response is returned.
        /// On success, the API returns <c>200 OK</c> along with the updated person data.
        /// Any unexpected errors will result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the person to update.</param>
        /// <param name="dto">The set of fields to partially update on the resource.</param>
        /// <returns>
        /// <c>Ok</c> with the updated person when the partial update succeeds,  
        /// <c>NotFound</c> if the person is not found,  
        /// <c>BadRequest</c> for validation issues, or  
        /// <c>InternalServerError</c> for unexpected exceptions.
        /// </returns>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<PersonPatchDto>))]
        public async Task<IActionResult> Patch(Guid id, [FromBody] PersonPatchDto dto)
        {
            var updated = await svc.PatchAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a person by its unique identifier.
        /// </summary>
        /// <remarks>
        /// Permanently removes the person from the system.
        ///
        /// If the person does not exist, a <c>404 Not Found</c> is returned.  
        /// On success, the endpoint returns <c>204 No Content</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifying the person to delete.</param>
        /// <response code="204">The person was successfully deleted.</response>
        /// <response code="404">Returned when the person does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await svc.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}