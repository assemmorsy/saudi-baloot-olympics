using BalootOlympicsTeamsApi.Modules.Players;

namespace BalootOlympicsTeamsApi.Modules.Teams;

public sealed class GetTeamsService(TeamRepo _teamRepo)
{
    public async Task<Result<List<Team>>> ExecuteAsync()
    {
        return await _teamRepo.GetAllAsync();
    }
}
public sealed class GetTeamsEndpoint : CarterModule
{
    public sealed record GetTeamDto(int Id, string Name, string State, List<GetPlayerEndpoint.PlayerDto> Players);

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/teams",
            async Task<IResult> (HttpContext context, [FromServices] GetTeamsService service) =>
                (await service.ExecuteAsync())
                    .ResolveToIResult(
                        (teams) => TypedResults.Ok(new SuccessResponse<List<GetTeamDto>>(
                                teams.Select(t => PlayersMapper.TeamToTeamDto(t)).ToList(),
                                "Teams fetched successfully."
                        )),
                        context.TraceIdentifier));
    }

}