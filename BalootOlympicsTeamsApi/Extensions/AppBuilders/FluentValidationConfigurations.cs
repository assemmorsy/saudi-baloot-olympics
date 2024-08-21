using System.Globalization;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace BalootOlympicsTeamsApi.Extensions.AppBuilders;

public static class FluentValidationConfigurations
{
    public static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {

        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.OverrideDefaultResultFactoryWith<ValidatorResultFactory>();
            // configuration.EnableFormBindingSourceAutomaticValidation = true;
            // configuration.EnableBodyBindingSourceAutomaticValidation = true;
            // configuration.EnablePathBindingSourceAutomaticValidation = true;
            // configuration.EnablePathBindingSourceAutomaticValidation = true;
            // configuration.EnableQueryBindingSourceAutomaticValidation = true;
            // configuration.EnableCustomBindingSourceAutomaticValidation = true;
        });

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ar");

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        return services;

    }

}
