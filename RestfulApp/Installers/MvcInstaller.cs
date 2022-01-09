using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Filters;
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
            ValidateLifetime = true
        };

        services.AddSingleton(jwtSettings)
                .AddSingleton(tokenValidationParameters)
                .AddScoped<IIdentityService, IdentityService>()
                .AddMvc(setup =>
                {
                    setup.EnableEndpointRouting = false;
                    setup.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(setup => setup.RegisterValidatorsFromAssemblyContaining<Program>()).Services
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
                });                
    }
}