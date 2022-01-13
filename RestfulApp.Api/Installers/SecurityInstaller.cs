using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Api.Settings;
using System.Text;

namespace RestfulApp.Api.Installers;

public class SecurityInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();
        configuration.Bind(nameof(JwtOptions), jwtOptions);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        var jwtSettings = new JwtSettings(jwtOptions, tokenValidationParameters);

        services.AddSingleton(jwtSettings)
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