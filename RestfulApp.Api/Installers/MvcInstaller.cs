using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Api.Filters;
using RestfulApp.Api.Services;
using RestfulApp.Api.Settings;
using RestfulApp.Api.Validation.Options;
using System.Text;

namespace RestfulApp.Api.Installers;

public class MvcInstaller : IInstaller
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

        services.Configure<ApiBehaviorOptions>(setup => setup.SuppressModelStateInvalidFilter = true)
                .AddSingleton(jwtSettings)
                .AddScoped<IUriService>(setup =>
                {
                    var accessor = setup.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var absoluteUri = $"{request.Scheme}://{request.Host}{request.Path}";
                    return new UriService(absoluteUri);
                })
                .AddScoped<IIdentityService, IdentityService>()
                .AddControllers(setup =>
                {
                    setup.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(setup =>
                {
                    setup.ValidatorOptions.PropertyNameResolver = LowerCamelCasePropertyNameResolver.ResolvePropertyName;
                    setup.RegisterValidatorsFromAssemblyContaining<Program>();
                }).Services
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