namespace BalootOlympicsTeamsApi.Entities;
public class Player
{
    public required string Id { get; set; }
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public PlayerState State { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}

public enum PlayerState
{
    Approved,
    Pending,
    Rejected,
}