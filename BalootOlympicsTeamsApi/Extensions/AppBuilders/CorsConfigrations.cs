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
                List<string> origins = ["https://test-signalr.netlify.app", "https://qydha-staging.netlify.app", "https://qayedhaadmin.web.app", "https://qydha.com"];
                if (!environment.IsProduction())
                    origins.AddRange(["http://localhost:5173", "https://localhost:5173", "http://localhost:3000", "http://localhost:4200"]);
                builder
                    .WithOrigins([.. origins])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        return MyAllowSpecificOrigins;
        #endregion
    }
}
