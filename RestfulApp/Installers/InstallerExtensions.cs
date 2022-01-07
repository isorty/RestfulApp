namespace RestfulApp.Installers;

public static class InstallerExtensions
{
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        typeof(Program).Assembly.ExportedTypes.Where(type => typeof(IInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                                      .Select(Activator.CreateInstance)
                                      .Cast<IInstaller>()
                                      .ToList()
                                      .ForEach(installer => installer.InstallServices(services, configuration));
    }
}