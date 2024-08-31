using System.Text;
using BalootOlympicsTeamsApi.Modules.Players;
using static BalootOlympicsTeamsApi.Modules.Groups.GenerateGroupsEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Teams;

public sealed class GetTeamsService(TeamRepo _teamRepo)
{
    public async Task<Result<List<Team>>> ExecuteAsync()
    {
        return await _teamRepo.GetAllApprovedAsync();
    }
}
public sealed class GetTeamsEndpoint : CarterModule
{
    public sealed record GetTeamDto(int Id, string Name, string State, int? GroupId, GetGroupDto? Group, List<GetPlayerEndpoint.PlayerDto> Players);
    public sealed record GetTeamWithoutPlayersDto(int Id, string Name, string State, int? GroupId);

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // app.MapGet("/teams",
        //     async Task<IResult> (HttpContext context, [FromServices] GetTeamsService service) =>
        //         (await service.ExecuteAsync())
        //             .ResolveToIResult(
        //                 (teams) => TypedResults.Ok(new SuccessResponse<List<GetTeamWithoutPlayersDto>>(
        //                         teams.Select(t => PlayersMapper.TeamToTeamWithoutPlayersDto(t)).ToList(),
        //                         "Teams fetched successfully."
        //                 )),
        //                 context.TraceIdentifier));

        // app.MapGet("/teams/string",
        //     async Task<IResult> (HttpContext context, [FromServices] GetTeamsService service) =>
        //         (await service.ExecuteAsync())
        //             .ResolveToIResult(
        //                 (teams) =>
        //                 {
        //                     var builder = new StringBuilder();
        //                     foreach (var team in teams)
        //                     {
        //                         builder.AppendLine(team.ToString());
        //                     }
        //                     return TypedResults.Text(builder.ToString());
        //                 },
        //                 context.TraceIdentifier));
    }

}