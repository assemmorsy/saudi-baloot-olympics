namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class RepoRegistrations
{
    public static IServiceCollection RegisterRepos(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {

        #region DI Services
        services.AddScoped<PlayerRepo>();
        services.AddScoped<ConfirmationRequestRepo>();
        services.Scan(scan => scan.FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repo")))
            .AsSelf()
            .WithScopedLifetime()
        );
        #endregion
        return services;

    }
}
