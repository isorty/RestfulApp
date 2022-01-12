using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestfulApp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private bool _isEnabled = true;

    public ApiKeyAuthAttribute() { }

    public ApiKeyAuthAttribute(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_isEnabled && !IsAuthorized(context))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    private bool IsAuthorized(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            return false;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = configuration.GetValue<string>(ApiKeyHeaderName);

        return apiKey.Equals(potentialApiKey, StringComparison.InvariantCultureIgnoreCase);
    }
}