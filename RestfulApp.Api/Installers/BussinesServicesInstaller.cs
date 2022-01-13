using RestfulApp.Api.Services;

namespace RestfulApp.Api.Installers;

public class BussinesServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor()
                .AddScoped<IUriService, UriService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IItemService, ItemService>()
                .AddAutoMapper(typeof(Program));
    }
}