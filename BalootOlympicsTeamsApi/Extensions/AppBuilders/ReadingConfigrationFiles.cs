namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class ReadingConfigurationFile
{
    public static ConfigurationManager ReadConfigurationFile(this ConfigurationManager configuration, IWebHostEnvironment environment)
    {
        configuration
            .AddJsonFile("AppKeys/app_keys.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"AppKeys/app_keys.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        return configuration;
    }
}
