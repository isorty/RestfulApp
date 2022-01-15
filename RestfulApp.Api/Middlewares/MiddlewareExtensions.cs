using RestfulApp.Api.Settings;

namespace RestfulApp.Api.Middlewares;

public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds a middleware that provides health check status.
    /// </summary>
    /// <param name="builder">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
    /// <param name="settings">The RestfulApp.Api.Settings.HealthCheckSettings.</param>
    /// <returns>A reference to the app after the operation has completed.</returns>
    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder builder, HealthCheckSettings settings)
    {
        if (settings.IsEnabled)
        {
            builder.UseHealthChecks(settings.Path, settings.Options);
        }

        return builder;
    }

    /// <summary>
    /// Register the Swagger and SwaggerUI middlewares with optional setup action for DI-injected options.
    /// 
    /// </summary>
    public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder builder, SwaggerOptions options)
    {
        builder.UseSwagger(setup => setup.RouteTemplate = options.JsonRoute);
        builder.UseSwaggerUI(setup => setup.SwaggerEndpoint(options.UiEndpoint, options.Description));

        return builder;
    }
}