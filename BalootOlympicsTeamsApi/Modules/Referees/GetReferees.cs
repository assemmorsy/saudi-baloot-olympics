using BalootOlympicsTeamsApi.Modules.Players;

namespace BalootOlympicsTeamsApi.Modules.Referees;

public sealed class GetRefereesService(OlympicsContext _dbCtx)
{
    public async Task<Result<List<Referee>>> ExecuteAsync()
    {
        return await _dbCtx.Referees.ToListAsync();
    }
}
public sealed class GetRefereesEndpoint : CarterModule
{
    public sealed record GetRefereeDto(Guid Id, string Name, string Phone, string Username);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // app.MapGet("/referees",
        //     async Task<IResult> (HttpContext context, [FromServices] GetRefereesService service) =>
        //         (await service.ExecuteAsync())
        //             .ResolveToIResult(
        //                 (Referees) =>
        //                 {
        //                     var res = new SuccessResponse<List<GetRefereeDto>>(
        //                         Referees.Select(r => PlayersMapper.RefereeToRefereeDto(r)).ToList(),
        //                         "Referees fetched successfully.");
        //                     return TypedResults.Ok(res);
        //                 },
        //                 context.TraceIdentifier));
    }

}