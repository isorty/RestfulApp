using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace RestfulApp.Api.Installers;

public class SwaggerInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Restful App", Version = "v1" });
                    setup.ExampleFilters();
                    setup.AddSecurityDefinition("Bearer", new()
                    {
                        Description = "JWT auth using bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    setup.AddSecurityRequirement(new()
                    {
                        {
                            new()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                    setup.IncludeXmlComments(
                        Path.Combine(
                            AppContext.BaseDirectory,
                            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                })
                .AddSwaggerExamplesFromAssemblyOf<Program>();
    }
}