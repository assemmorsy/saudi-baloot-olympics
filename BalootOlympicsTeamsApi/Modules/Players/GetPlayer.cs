namespace BalootOlympicsTeamsApi.Modules.Players;

public sealed class GetPlayerService(PlayerRepo _playerRepo, TeamRepo _teamRepo)
{
    private readonly Func<Player, Task<Result<Player>>> PopulateTeam = async (player) =>
    {
        if (player.TeamId != null)
            (await _teamRepo.GetByIdAsync(player.TeamId.Value))
                .OnSuccess(team => player.Team = team);
        return Result.Ok(player);
    };
    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByPhoneDto dto) =>
        (await _playerRepo.GetPlayerByAsync(p => p.Phone == dto.Phone, dto.Phone))
        .OnSuccessAsync(PopulateTeam);
    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByEmailDto dto) =>
        (await _playerRepo.GetPlayerByAsync(p => p.Email == dto.Email, dto.Email))
        .OnSuccessAsync(PopulateTeam);

    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByIdDto dto) =>
        (await _playerRepo.GetPlayerByAsync(p => p.Id == dto.Id, dto.Id))
        .OnSuccessAsync(PopulateTeam);

}
public sealed class GetPlayerEndpoint : CarterModule
{
    public sealed record GetPlayerByPhoneDto(string Phone);
    public sealed record GetPlayerByIdDto(string Id);
    public sealed record GetPlayerByEmailDto(string Email);
    public sealed record PlayerDto(string Id, string Name, string Phone, string Email, string State, string Comment, int? TeamId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        static IResult ResolveResponse(Player player)
        {
            return TypedResults.Ok(new SuccessResponse(new
            {
                Player = PlayersMapper.PlayerToPlayerDto(player),
                Team = player.TeamId != null && player.Team != null ? PlayersMapper.TeamToTeamDto(player.Team) : null
            }, "player fetched successfully."));

        }

        app.MapGet("/players/phone/{phone}",
                async Task<IResult> (string phone, HttpContext context, [FromServices] GetPlayerService service) =>
                {
                    return (await service.ExecuteAsync(new GetPlayerByPhoneDto(phone)))
                    .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
                });

        app.MapGet("/players/email/{email}",
            async Task<IResult> (string email, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.ExecuteAsync(new GetPlayerByEmailDto(email)))
                .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
            });
        app.MapGet("/players/{id}",
            async Task<IResult> (string id, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.ExecuteAsync(new GetPlayerByIdDto(id)))
                .ResolveToIResult(ResolveResponse, context.TraceIdentifier);
            });

    }

}