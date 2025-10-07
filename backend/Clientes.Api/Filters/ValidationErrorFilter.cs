using Clientes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clientes.Api.Filters;

public class ValidationErrorFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .SelectMany(kvp => kvp.Value!.Errors.Select(error =>
                    new ApiError("VALIDATION_ERROR", error.ErrorMessage, kvp.Key)));

            var response = ApiResponse<object>.Failure(errors, context.HttpContext.TraceIdentifier);
            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        await next();
    }
}
