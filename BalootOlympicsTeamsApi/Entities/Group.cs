namespace BalootOlympicsTeamsApi.Entities;
public class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateTimeOffset CheckInAt { get; set; }
    public required DateTimeOffset StartPlayAt { get; set; }
    public required List<Team> CompetingTeams { get; set; }
    public List<Match> Matches { get; set; } = [];
}