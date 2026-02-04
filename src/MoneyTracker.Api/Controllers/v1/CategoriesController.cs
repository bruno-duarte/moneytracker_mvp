using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs.Categories;
using Swashbuckle.AspNetCore.Annotations;
using MoneyTracker.Api.Responses;
using MoneyTracker.Api.Filters;

namespace MoneyTracker.Api.Controllers
{
    /// <summary>
    /// API controller for managing categories used in transactions.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for creating, listing, retrieving, updating,
    /// and deleting categories.
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("API controller for managing categories used in transactions.")]
    public class CategoriesController(ICategoryService svc) : ControllerBase
    {
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new category with the provided name.
        ///
        /// On success, it returns a <c>201 Created</c> response along with
        /// the created category and a <c>Location</c> header pointing to
        /// the <see cref="GetById"/> endpoint.
        /// </remarks>
        /// <param name="dto">The data required to create the category.</param>
        /// <response code="201">The category was successfully created.</response>
        /// <response code="400">Returned when the request body is invalid.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CategorySaveDto dto)
        {
            var c = await svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }

        /// <summary>
        /// Retrieves a paginated list of categories.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a paginated collection of category records, optionally
        /// filtered based on the properties provided through the query string using
        /// <see cref="CategoryQueryDto"/>.
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
        /// A paginated list of categories, or an error response in case of validation
        /// issues or unhandled exceptions.
        /// </returns>
        /// <response code="200">Returns the filtered and paginated list of categories.</response>
        /// <response code="400">Validation errors in the query parameters.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> List([FromQuery] CategoryQueryDto query)
        {
            var result = await svc.ListAsync(query);

            var items = result.Items.ToList();

            Response.Headers.Append("X-Pagination-TotalCount", result.TotalCount.ToString());
            Response.Headers.Append("X-Pagination-PageNumber", result.PageNumber.ToString());
            Response.Headers.Append("X-Pagination-PageSize", result.PageSize.ToString());

            return Ok(items);
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <remarks>
        /// If the category exists, it is returned with a <c>200 OK</c> response.  
        /// If not found, a <c>404 Not Found</c> is returned.
        /// </remarks>
        /// <param name="id">The unique GUID identifying the category.</param>
        /// <response code="200">Returns the matching category.</response>
        /// <response code="404">Returned when the category does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var c = await svc.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        /// <summary>
        /// Fully updates an existing category by replacing all of its data.
        /// </summary>
        /// <remarks>
        /// This operation follows REST <c>PUT</c> semantics, meaning that the entire
        /// category resource is replaced using the data provided in the request body.
        /// All fields in <paramref name="dto"/> must be supplied, as partial updates
        /// are not supported through this endpoint (use <c>PATCH</c> for partial updates).
        ///
        /// If the specified category does not exist, a <c>404 Not Found</c> response is returned.
        /// If validation fails, the request results in a <c>400 Bad Request</c>.
        ///
        /// On success, the endpoint returns <c>200 OK</c> along with the updated
        /// category data.  
        /// Unexpected server errors result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the category to update.</param>
        /// <param name="dto">The full set of category fields used to replace the existing resource.</param>
        /// <returns>
        /// <c>Ok</c> with the updated category when the replacement succeeds,  
        /// <c>NotFound</c> if the category is not found,  
        /// <c>BadRequest</c> for validation issues, or  
        /// <c>InternalServerError</c> for unexpected exceptions.
        /// </returns>
        /// <response code="200">The category was successfully updated and returned in the response body.</response>
        /// <response code="400">Returned when validation of the input data fails.</response>
        /// <response code="404">Returned when the category does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategorySaveDto dto)
        {
            var updated = await svc.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Partially updates an existing category.
        /// </summary>
        /// <remarks>
        /// This operation follows REST <c>PATCH</c> semantics, applying only the
        /// fields provided in <paramref name="dto"/> to the existing resource.
        /// Fields omitted from the request body will remain unchanged.
        ///
        /// If the specified category does not exist, a <c>404 Not Found</c> response is returned.
        /// If validation fails, a <c>400 Bad Request</c> response is returned.
        /// On success, the API returns <c>200 OK</c> along with the updated category data.
        /// Any unexpected errors will result in a <c>500 Internal Server Error</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifier of the category to update.</param>
        /// <param name="dto">The set of fields to partially update on the resource.</param>
        /// <returns>
        /// <c>Ok</c> with the updated category when the partial update succeeds,  
        /// <c>NotFound</c> if the category is not found,  
        /// <c>BadRequest</c> for validation issues, or  
        /// <c>InternalServerError</c> for unexpected exceptions.
        /// </returns>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ServiceFilter(typeof(ValidationFilterAttribute<CategoryPatchDto>))]
        public async Task<IActionResult> Patch(Guid id, [FromBody] CategoryPatchDto dto)
        {
            var updated = await svc.PatchAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a category by its unique identifier.
        /// </summary>
        /// <remarks>
        /// Permanently removes the category from the system.
        ///
        /// If the category does not exist, a <c>404 Not Found</c> is returned.  
        /// On success, the endpoint returns <c>204 No Content</c>.
        /// </remarks>
        /// <param name="id">The unique GUID identifying the category to delete.</param>
        /// <response code="204">The category was successfully deleted.</response>
        /// <response code="404">Returned when the category does not exist.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await svc.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
