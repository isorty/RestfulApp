using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestfulApp.Options;
using RestfulApp.Services;
using System.Text;

namespace RestfulApp.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true,
            //ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(jwtSettings)
                .AddSingleton(tokenValidationParameters)
                .AddScoped<IIdentityService, IdentityService>()
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    setup.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(setup =>
                {
                    setup.SaveToken = true;
                    setup.TokenValidationParameters = tokenValidationParameters;
                }).Services
                .AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Restful App", Version = "v1" }); 

                    var security = new Dictionary<string, IEnumerable<string>>()
                    {
                        {"Bearer", new string[0] }
                    };

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
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });
    }
}