namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class SettingsRegistrations
{
    public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
    {

        #region DI settings
        services.Configure<EmailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<WhatsAppSettings>(configuration.GetSection("WhatsAppSettings"));
        services.Configure<OTPSettings>(configuration.GetSection("OTP"));
        #endregion
        return services;
    }
}
