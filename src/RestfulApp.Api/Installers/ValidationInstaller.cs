using FluentValidation;
using FluentValidation.AspNetCore;
using RestfulApp.Api.Validation.Options;

namespace RestfulApp.Api.Installers;

public sealed class ValidationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<Program>();

        ValidatorOptions.Global.PropertyNameResolver = LowerCamelCasePropertyNameResolver.ResolvePropertyName;
    }
}
