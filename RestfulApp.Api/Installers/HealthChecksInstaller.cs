using RestfulApp.Api.Data;

namespace RestfulApp.Api.Installers;

public class HealthChecksInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<DataContext>();
    }
}