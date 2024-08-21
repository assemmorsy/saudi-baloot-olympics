namespace BalootOlympicsTeamsApi.Entities;
public class Team
{
    public int Id { get; set; }
    public string Name
    {
        get { return string.Join(" | ", Players.Select(p => p.NameAr.Split()[0])); }
    }
    public List<Player> Players { get; set; } = [];
}