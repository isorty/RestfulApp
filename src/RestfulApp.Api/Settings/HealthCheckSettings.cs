using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RestfulApp.Api.Contracts.HealthChecks;
using System.Net.Mime;

namespace RestfulApp.Api.Settings;

public class HealthCheckSettings
{
    public bool IsEnabled { get; set; }
    public string Path { get; set; }

    public HealthCheckOptions Options =>
        new()
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(r => new HealthCheck
                    {
                        Component = r.Key,
                        Status = r.Value.Status.ToString(),
                        Description = r.Value.Description
                    }),
                    Duration = report.TotalDuration
                };

                await context.Response.WriteAsync(response.ToString());
            }
        };
}