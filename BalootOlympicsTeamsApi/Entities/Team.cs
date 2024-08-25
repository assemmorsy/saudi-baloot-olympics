namespace BalootOlympicsTeamsApi.Entities;
public class Team
{
    public int Id { get; set; }
    public string Name
    {
        get { return string.Join(" | ", Players.Select(p => string.Join(" ", [p.NameAr.Split()[0], p.NameAr.Split()[1]]))); }
    }

    public PlayerState State
    {
        get
        {
            if (Players.Any(p => p.State == PlayerState.Rejected))
                return PlayerState.Rejected;
            else if (Players.Any(p => p.State == PlayerState.Pending))
                return PlayerState.Pending;
            else
                return PlayerState.Approved;
        }
    }
    public List<Player> Players { get; set; } = [];
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }
}