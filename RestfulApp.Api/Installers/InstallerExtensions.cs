namespace RestfulApp.Api.Installers;

public static class InstallerExtensions
{
    /// <summary>
    /// Searchs for every realization of <c>IInstaller</c> interface,
    /// creates instances and invokes <c>InstallServices</c> method.
    /// </summary>
    /// <typeparam name="T"><c>IInstaller</c> realizations are searched in assembly
    /// where <c>T</c> type is declared.
    /// </typeparam>
    public static void InstallServicesInAssembly<T>(this IServiceCollection services, IConfiguration configuration)
    {
        typeof(T).Assembly.ExportedTypes
            .Where(expType => IsInterfaceRealization(expType, typeof(IInstaller)))
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList()
            .ForEach(installer => installer.InstallServices(services, configuration));
    }

    /// <summary>
    /// Check if a given type is a realization of a given interface.
    /// </summary>
    /// <param name="type">Class type.</param>
    /// <param name="interfaceType">Interface type.</param>
    private static bool IsInterfaceRealization(Type type, Type interfaceType) =>
        interfaceType.IsInterface &&
        !(type.IsInterface || type.IsAbstract) &&
        interfaceType.IsAssignableFrom(type);
}