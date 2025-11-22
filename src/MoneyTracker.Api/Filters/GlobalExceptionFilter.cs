using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyTracker.Api.Responses;
using MoneyTracker.Application.Exceptions;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is EntityNotFoundException nf)
        {
            var error = new ErrorResponse(
                StatusCodes.Status404NotFound,
                "Resource not found",
                nf.Message
            );

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            return;
        }

        if (context.Exception is InvalidReferenceException re)
        {
            var error = new ErrorResponse(
                StatusCodes.Status400BadRequest,
                "Invalid reference",
                re.Message
            );

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            return;
        }

        var genericError = new ErrorResponse(
            StatusCodes.Status500InternalServerError,
            "Internal server error",
            context.Exception.Message
        )
        {
            Errors = new Dictionary<string, string[]>
            {
                { "Exception", new[] { context.Exception.Message } }
            }
        };

        context.Result = new ObjectResult(genericError)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}
