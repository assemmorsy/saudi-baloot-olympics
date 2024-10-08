using System.Text.Json;

namespace BalootOlympicsTeamsApi.Entities;
public static class GenerationConstants
{
    public const double HoursBetweenCheckInAndPlayOfTheGroup = 1;
    public const double HoursBetweenEachGroup = 0.5;
    public const double MatchDurationInHours = 0.5;
}


public static class SerializationConstants
{

    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}