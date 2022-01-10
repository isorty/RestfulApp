using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RestfulApp.Extensions;

public static class GeneralExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User is null)
        {
            return string.Empty;
        }

        return httpContext.User.Claims.Single(claim => claim.Type == "id").Value;
    }

    public static bool IsInvalid(this ModelStateDictionary modelState) => !modelState.IsValid;

    public static string ToLowerCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Length == 1)
        {
            return value.ToLower();
        }

        return value[0..1].ToLower() + value[1..];
    }
}