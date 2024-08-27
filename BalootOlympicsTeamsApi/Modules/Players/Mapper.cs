using Riok.Mapperly.Abstractions;
using static BalootOlympicsTeamsApi.Modules.Groups.GenerateGroupsEndpoint;
using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;
using static BalootOlympicsTeamsApi.Modules.Referees.GetRefereesEndpoint;
using static BalootOlympicsTeamsApi.Modules.Teams.GetTeamsEndpoint;
namespace BalootOlympicsTeamsApi.Modules.Players;

[Mapper]
public static partial class PlayersMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(Player.NameEn))]
    [MapProperty(nameof(Player.NameAr), nameof(GetPlayerEndpoint.PlayerDto.Name))]
    public static partial GetPlayerEndpoint.PlayerDto PlayerToPlayerDto(this Player player);

    [MapperIgnoreSource(nameof(Team.Group))]
    public static partial GetTeamDto TeamToTeamDto(this Team team);

    [MapperIgnoreSource(nameof(Team.Players))]
    [MapperIgnoreSource(nameof(Team.Group))]
    public static partial GetTeamWithoutPlayersDto TeamToTeamWithoutPlayersDto(this Team team);

    [MapperIgnoreSource(nameof(Group.Matches))]
    [MapperIgnoreSource(nameof(Group.CompetingTeams))]
    public static partial GetGroupDto GroupToGroupDto(this Group group);

    [MapperIgnoreSource(nameof(Match.Group))]
    [MapperIgnoreSource(nameof(Match.Referee))]
    // [MapperIgnoreSource(nameof(Match.UsTeam))]
    // [MapperIgnoreSource(nameof(Match.ThemTeam))]
    [MapperIgnoreSource(nameof(Match.MatchQualifyUsTeam))]
    [MapperIgnoreSource(nameof(Match.MatchQualifyThemTeam))]

    public static partial GetMatchWithoutPlayersDto MatchToMatchDto(this Match match);


    [MapperIgnoreSource(nameof(Match.Group))]
    [MapperIgnoreSource(nameof(Match.Referee))]
    // [MapperIgnoreSource(nameof(Match.UsTeam))]
    // [MapperIgnoreSource(nameof(Match.ThemTeam))]
    [MapperIgnoreSource(nameof(Match.MatchQualifyUsTeam))]
    [MapperIgnoreSource(nameof(Match.MatchQualifyThemTeam))]
    public static partial GetMatchWithPlayersDto MatchToMatchWithPlayersDto(this Match match);

    public static partial GetRefereeDto RefereeToRefereeDto(this Referee match);


}