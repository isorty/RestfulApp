using Microsoft.EntityFrameworkCore;
using RestfulApp.Data;
using RestfulApp.Installers;
using RestfulApp.Settings;

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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var swaggerOptions = new SwaggerOptions();
        configuration.Bind(nameof(SwaggerOptions), swaggerOptions);

        app.UseSwagger(setup => setup.RouteTemplate = swaggerOptions.JsonRoute);
        app.UseSwaggerUI(setup => setup.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}