namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class DbConfigurationExtension
{
    public static void DbConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("postgres");
        services.AddDbContext<OlympicsContext>(
            (opt) =>
            {
                var options = opt.UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                if (!environment.IsProduction())
                    options.EnableSensitiveDataLogging();
            }
        );
    }
}
