namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class CorsConfigurations
{
    public static string ConfigureCORS(this IServiceCollection services, IWebHostEnvironment environment)
    {
        #region Add Cors
        string MyAllowSpecificOrigins = "_MyAllowSpecificOrigins";

        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return MyAllowSpecificOrigins;
        #endregion
    }
}
