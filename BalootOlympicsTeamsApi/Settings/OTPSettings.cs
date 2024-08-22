namespace BalootOlympicsTeamsApi.Settings;

public class OTPSettings
{
    public string Secret { get; set; } = null!;
    public int TimeInSec { get; set; } = 500;
}
