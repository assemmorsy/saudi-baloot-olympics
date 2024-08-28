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
                if (environment.IsProduction())
                    builder
                        .WithOrigins("https://test-signalr.netlify.app", "https://qayedhaadmin.web.app", "https://qydha.com");
                else
                    builder
                        .WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost:3000", "http://localhost:4200", "https://qayedhaadmin.web.app");
                builder
                    // .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return MyAllowSpecificOrigins;
        #endregion
    }
}
