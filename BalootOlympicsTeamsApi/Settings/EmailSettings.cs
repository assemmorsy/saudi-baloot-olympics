namespace BalootOlympicsTeamsApi.Settings;

public class EmailSettings
{
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Token { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;


}
