using FluentValidation.AspNetCore;
using RestfulApp.Api.Filters;
using RestfulApp.Api.Validation.Options;

namespace RestfulApp.Api.Installers;

public class ControllersInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(setup =>
                {
                    setup.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(setup =>
                {
                    setup.ValidatorOptions.PropertyNameResolver = LowerCamelCasePropertyNameResolver.ResolvePropertyName;
                    setup.RegisterValidatorsFromAssemblyContaining<Program>();
                });
    }
}