using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Api.Data;
using RestfulApp.Api.Installers;
using RestfulApp.Api.Settings;
using RestfulApp.Contracts.HealthChecks;
using System.Net.Mime;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.InstallServicesInAssembly<Program>(configuration);
        builder.Services.AddAutoMapper(typeof(Program));

        var app = builder.Build();

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

        app.UseHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(r => new HealthCheck
                    {
                        Component = r.Key,
                        Status = r.Value.Status.ToString(),
                        Description = r.Value.Description
                    }),
                    Duration = report.TotalDuration
                };

                await context.Response.WriteAsync(response.ToString());
            }
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var swaggerOptions = new SwaggerOptions();
        configuration.Bind(nameof(SwaggerOptions), swaggerOptions);

        app.UseSwagger(setup => setup.RouteTemplate = swaggerOptions.JsonRoute);
        app.UseSwaggerUI(setup => setup.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}