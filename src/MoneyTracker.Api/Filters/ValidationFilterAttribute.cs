using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyTracker.Api.Responses;

namespace MoneyTracker.Api.Filters
{
    public class ValidationFilterAttribute<T>(IValidator<T> validator) : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator = validator;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dto = context.ActionArguments
                .FirstOrDefault(kvp => kvp.Value is T)
                .Value as T;

            if (dto is null)
            {
                await next();
                return;
            }

            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                var error = new ErrorResponse(
                    StatusCodes.Status400BadRequest,
                    "Validation failed",
                    "One or more validation errors occurred."
                )
                {
                    Errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                };

                context.Result = new BadRequestObjectResult(error);
                return;
            }

            await next();
        }
    }
}
