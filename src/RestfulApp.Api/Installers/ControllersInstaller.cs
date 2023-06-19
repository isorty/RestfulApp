using RestfulApp.Api.Filters;

namespace RestfulApp.Api.Installers;

public class ControllersInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(setup => setup.Filters.Add<ValidationFilter>());
    }
}