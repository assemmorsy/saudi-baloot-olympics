namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;


public static class MediatRConfigurations
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {

        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
        return services;

    }
}
