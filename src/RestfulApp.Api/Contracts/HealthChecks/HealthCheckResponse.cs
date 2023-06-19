using System.Text.Json;

namespace RestfulApp.Api.Contracts.HealthChecks;

public sealed class HealthCheckResponse
{
    public string Status { get; set; }
    public IEnumerable<HealthCheck> Checks { get; set; }
    public TimeSpan Duration { get; set; }

    public HealthCheckResponse()
    {

    }

    public HealthCheckResponse(string status, IEnumerable<HealthCheck> checks, TimeSpan duration)
    {
        Status = status;
        Checks = checks;
        Duration = duration;
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}