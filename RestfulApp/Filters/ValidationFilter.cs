using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Extensions;

namespace RestfulApp.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsInvalid())
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage));

            var errorResponse = new ErrorResponse();

            foreach (var error in errorsInModelState)
            {
                foreach (var suberror in error.Value)
                {
                    var errorModel = new ErrorModel
                    {
                        FieldName = error.Key,
                        Message = suberror
                    };

                    errorResponse.Errors.Add(errorModel);
                }
            }

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}