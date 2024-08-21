namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class ServicesRegistrations
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {

        #region DI Services
        services.AddScoped<MailingService>();
        services.AddScoped<WhatsAppService>();
        services.AddSingleton<OtpManager>();
        services.AddHttpClient();
        services.Scan(scan => scan.FromCallingAssembly()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsSelf()
            .WithScopedLifetime()
        );
        #endregion
        return services;

    }
}
