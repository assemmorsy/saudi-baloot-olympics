using System.Linq.Expressions;
using static BalootOlympicsTeamsApi.Modules.Players.GetPlayerService;

namespace BalootOlympicsTeamsApi.Modules.Players;

public sealed class GetPlayerService(OlympicsContext _dbCtx)
{
    public record PlayerStateData(Player Player, Team? Team, int? Level);
    public async Task<Result<PlayerStateData>> Get<T>(Expression<Func<Player, bool>> predicate, T identifier)
    {
        Player? player = await _dbCtx.Players
          .Include(p => p.Team)
          .SingleOrDefaultAsync(predicate);
        if (player == null) return Result.Fail(new EntityNotFoundError<T>(identifier, nameof(Player)));
        if (player.TeamId == null) return Result.Ok(new PlayerStateData(player, null, null));
        Team? team = await _dbCtx.Teams
           .Include(t => t.Players)
           .Include(t => t.Group)
           .SingleOrDefaultAsync(t => t.Id == player.TeamId);
        if (team == null) return Result.Fail(new EntityNotFoundError<int>(player.TeamId.Value, nameof(Team)));
        player.Team = team;
        var matches = await _dbCtx.Matches
            .Where(m => m.UsTeamId == team.Id || m.ThemTeamId == team.Id)
            .OrderByDescending(m => m.Level).Take(1)
            .ToListAsync();
        if (matches.Count == 0) return Result.Ok(new PlayerStateData(player, team, null));

        return Result.Ok(new PlayerStateData(player, team, matches.First().Level));
    }
}
public sealed class GetPlayerEndpoint : CarterModule
{
    public sealed record PlayerDto(string Id, string Name, string Phone, string Email, string State, string Comment, int? TeamId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        static IResult ResolveResponse(PlayerStateData data)
        {
            return TypedResults.Ok(new SuccessResponse(new
            {
                Player = PlayersMapper.PlayerToPlayerDto(data.Player),
                Team = data.Team != null ? PlayersMapper.TeamToTeamDto(data.Team) : null,
                data.Level
            }, "player fetched successfully."));
        }

        app.MapGet("/players/phone/{phone}",
                async Task<IResult> (string phone, HttpContext context, [FromServices] GetPlayerService service) =>
                {
                    return (await service.Get((p) => p.Phone == phone.Trim(), phone))
                    .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
                });

        app.MapGet("/players/email/{email}",
            async Task<IResult> (string email, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.Get((p) => p.Email == email.Trim(), email))
                   .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
            });
        app.MapGet("/players/{id}",
            async Task<IResult> (string id, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.Get((p) => p.Id == id.Trim(), id))
                   .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
            });

    }

}