namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class RepoRegistrations
{
    public static IServiceCollection RegisterRepos(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {

        #region DI Services
        services.AddScoped<PlayerRepo>();
        services.AddScoped<ConfirmationRequestRepo>();

        #endregion
        return services;

    }
}
