using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public static class ConfigurationUtils
{
    public static ValueConverter DateTimeOffsetValueConverter = new ValueConverter<DateTimeOffset, DateTimeOffset>(
        v => v.UtcDateTime,
        v => v.ToUniversalTime());
}