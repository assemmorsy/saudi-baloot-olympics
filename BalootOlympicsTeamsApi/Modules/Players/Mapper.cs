using Riok.Mapperly.Abstractions;
using static BalootOlympicsTeamsApi.Modules.Teams.GetTeamsEndpoint;
namespace BalootOlympicsTeamsApi.Modules.Players;

[Mapper]
public static partial class PlayersMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(Player.NameEn))]
    [MapProperty(nameof(Player.NameAr), nameof(GetPlayerEndpoint.PlayerDto.Name))]
    public static partial GetPlayerEndpoint.PlayerDto PlayerToPlayerDto(this Player player);

    public static partial GetTeamDto TeamToTeamDto(this Team team);

    [MapperIgnoreSource(nameof(Team.Players))]
    public static partial GetTeamWithoutPlayersDto TeamToTeamWithoutPlayersDto(this Team team);

}