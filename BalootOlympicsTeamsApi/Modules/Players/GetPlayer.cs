namespace BalootOlympicsTeamsApi.Modules.Players;

public sealed class GetPlayerService(PlayerRepo _playerRepo)
{
    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByPhoneDto dto) =>
        await _playerRepo.GetPlayerByPhoneAsync(dto.Phone);
    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByEmailDto dto) =>
        await _playerRepo.GetPlayerByEmailAsync(dto.Email);
    public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByIdDto dto) =>
        await _playerRepo.GetPlayerByIdAsync(dto.Id);
}
public sealed class GetPlayerEndpoint : CarterModule
{
    public sealed record GetPlayerByPhoneDto(string Phone);
    public sealed record GetPlayerByIdDto(string Id);
    public sealed record GetPlayerByEmailDto(string Email);
    public sealed record PlayerDto(string Id, string Name, string Phone, string Email, string State, string Comment, int? TeamId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/players/phone/{phone}",
            async Task<IResult> (string phone, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.ExecuteAsync(new GetPlayerByPhoneDto(phone)))
                .ResolveToIResult((player) =>
                {
                    return TypedResults.Ok(
                        new SuccessResponse<PlayerDto>(
                            PlayersMapper.PlayerToPlayerDto(player),
                            "player fetched successfully.")
                        );
                }, context.TraceIdentifier);
            });

        app.MapGet("/players/email/{email}",
            async Task<IResult> (string email, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.ExecuteAsync(new GetPlayerByEmailDto(email)))
                .ResolveToIResult((player) =>
                {
                    return TypedResults.Ok(
                        new SuccessResponse<PlayerDto>(
                            PlayersMapper.PlayerToPlayerDto(player),
                            "player fetched successfully.")
                        );
                }, context.TraceIdentifier);
            });
        app.MapGet("/players/{id}",
            async Task<IResult> (string id, HttpContext context, [FromServices] GetPlayerService service) =>
            {
                return (await service.ExecuteAsync(new GetPlayerByIdDto(id)))
                .ResolveToIResult((player) =>
                {
                    return TypedResults.Ok(
                        new SuccessResponse<PlayerDto>(
                            PlayersMapper.PlayerToPlayerDto(player),
                            "player fetched successfully.")
                        );
                }, context.TraceIdentifier);
            });

    }

}