namespace BalootOlympicsTeamsApi.Modules.Players;

public static class GetPlayer
{
    public sealed class Service(PlayerRepo _playerRepo)
        : IAsyncCommandService<GetPlayerEndpoint.GetPlayerByPhoneDto, Result<Player>>
    {
        public async Task<Result<Player>> ExecuteAsync(GetPlayerEndpoint.GetPlayerByPhoneDto dto) =>
            await _playerRepo.GetPlayerByPhoneAsync(dto.Phone);
    }
}

public sealed class GetPlayerEndpoint : CarterModule
{
    public sealed record GetPlayerByPhoneDto(string Phone);
    public sealed record PlayerDto(string Id, string Name, string Phone, string Email, string State, string Comment, int? TeamId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/players/{phone}",
            async Task<IResult> (string phone, HttpContext context, IAsyncCommandService<GetPlayerByPhoneDto, Result<Player>> service) =>
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
    }
}