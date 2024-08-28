using BalootOlympicsTeamsApi.Modules.Players;
using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Referees;

public sealed class GetRefereeMatchesService(OlympicsContext _dbCtx)
{
    public async Task<Result<List<Match>>> ExecuteAsync(Guid refereeId)
    {
        var matches = await _dbCtx.Matches
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .AsSplitQuery()
            .AsTracking()
            .Where(m => m.RefereeId == refereeId)
            .OrderBy(m => m.StartAt)
            .ToListAsync();

        List<int> teamsIds = [];
        matches.ForEach(m =>
        {
            if (m.UsTeamId != null)
                teamsIds.Add(m.UsTeamId.Value);
            if (m.ThemTeamId != null)
                teamsIds.Add(m.ThemTeamId.Value);
        });
        teamsIds = teamsIds.Distinct().ToList();
        await _dbCtx.Players.AsTracking().Where(p => p.TeamId != null && teamsIds.Contains(p.TeamId.Value)).ToListAsync();
        // await _dbCtx.SaveChangesAsync();
        return Result.Ok(matches);
    }
}

public sealed class GetRefereeMatchesEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/referee/{referee_id}/matches",
            async Task<IResult> (Guid referee_id, HttpContext context, [FromServices] GetRefereeMatchesService service) =>
            {
                return (await service.ExecuteAsync(referee_id))
                .ResolveToIResult(matches =>
                {
                    var res = new SuccessResponse<List<GetMatchWithPlayersDto>>(
                        matches.Select(m => PlayersMapper.MatchToMatchWithPlayersDto(m)).ToList(),
                        "matches fetched successfully.");
                    return TypedResults.Ok(res);
                }, context.TraceIdentifier);
            })
            .AddFluentValidationAutoValidation();

    }

}