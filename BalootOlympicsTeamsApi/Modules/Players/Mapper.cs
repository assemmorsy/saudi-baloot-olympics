using Riok.Mapperly.Abstractions;
namespace BalootOlympicsTeamsApi.Modules.Players;

[Mapper]
public static partial class PlayersMapper
{
    [MapperIgnoreSource(nameof(Player.Team))]
    [MapperIgnoreSource(nameof(Player.NameEn))]
    [MapProperty(nameof(Player.NameAr), nameof(GetPlayerEndpoint.PlayerDto.Name))]
    public static partial GetPlayerEndpoint.PlayerDto PlayerToPlayerDto(this Player player);
}