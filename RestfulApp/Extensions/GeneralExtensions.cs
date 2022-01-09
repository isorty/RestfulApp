﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

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
}