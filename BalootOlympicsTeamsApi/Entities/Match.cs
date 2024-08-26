namespace BalootOlympicsTeamsApi.Entities;
public class Match
{
    public int Id { get; set; }
    public MatchState State { get; set; } = MatchState.Created;
    public required int Level { get; set; }
    public int TableNumber { get; set; }
    public Group Group { get; set; } = null!;
    public required int GroupId { get; set; }
    public Guid? QydhaGameId { get; set; }
    public Guid? RefereeId { get; set; }
    public required DateTimeOffset StartAt { get; set; }

    public MatchSide? Winner { get; set; }

    public Team? UsTeam { get; set; }
    public int? UsTeamId { get; set; }
    public Match? MatchQualifyUsTeam { get; set; }
    public int? MatchQualifyUsTeamId { get; set; }

    public Team? ThemTeam { get; set; } = null!;
    public int? ThemTeamId { get; set; }
    public Match? MatchQualifyThemTeam { get; set; } = null!;
    public int? MatchQualifyThemTeamId { get; set; } = null!;


}
public enum MatchState
{
    Created,
    Running,
    Ended
}

public enum MatchSide
{
    Us,
    Them,
    TwoTeamsWithdraw
}

