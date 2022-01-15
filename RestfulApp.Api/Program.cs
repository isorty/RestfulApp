using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Api.Data;
using RestfulApp.Api.Installers;
using RestfulApp.Api.Middlewares;
using RestfulApp.Api.Settings;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        builder.Services.Configure<ApiBehaviorOptions>(setup => setup.SuppressModelStateInvalidFilter = true);

        // Add services to the container.
        builder.Services.InstallServicesInAssembly<Program>(configuration);
        
        var app = builder.Build();

        // Bind configurations
        var healthCheckSettings = new HealthCheckSettings();
        configuration.Bind(nameof(HealthCheckSettings), healthCheckSettings);

        var swaggerOptions = new SwaggerOptions();
        configuration.Bind(nameof(SwaggerOptions), swaggerOptions);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            using var serviceScope = app.Services.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            await dataContext.Database.MigrateAsync();
        }
        else
        {
            app.UseHsts();
        }        

        app.UseHealthChecks(healthCheckSettings);

        app.UseHttpsRedirection();
        app.UseStaticFiles();        

        app.UseSwaggerWithUI(swaggerOptions);

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        await app.RunAsync();
    }
}