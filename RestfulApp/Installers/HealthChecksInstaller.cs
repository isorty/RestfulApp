using RestfulApp.Data;

namespace RestfulApp.Installers;

public class HealthChecksInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<DataContext>();
    }
}