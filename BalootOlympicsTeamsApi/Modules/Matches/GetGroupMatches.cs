using BalootOlympicsTeamsApi.Modules.Players;
using static BalootOlympicsTeamsApi.Modules.Teams.GetTeamsEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Matches;

public sealed class GetGroupMatchesService(OlympicsContext _dbCtx)
{
    public async Task<Result<List<Match>>> ExecuteAsync(int groupId)
    {
        var group = await _dbCtx.Groups
            .Include(g => g.CompetingTeams)
            .ThenInclude(t => t.Players)
            .AsSplitQuery()
            .AsTracking()
            .SingleOrDefaultAsync(g => g.Id == groupId);
        if (group == null) return Result.Fail(new EntityNotFoundError<int>(groupId, nameof(Group)));
        var matches = await _dbCtx.Matches
            .Where(m => m.GroupId == groupId)
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .AsSplitQuery()
            .AsTracking()
            .OrderByDescending(m => m.Level).ThenBy(m => m.TableNumber)
            .ToListAsync();
        return Result.Ok(matches);
    }
}

public sealed class GetGroupMatchesEndpoint : CarterModule
{
    public record GetGroupMatchesRequestDto(int QualifiedTeamsCount);
    public record GetMatchWithoutPlayersDto(int Id, string State, int Level, int TableNumber, int GroupId, Guid? QydhaGameId, Guid? RefereeId, DateTimeOffset StartAt, string? Winner, int? UsTeamId, GetTeamWithoutPlayersDto? UsTeam, int? ThemTeamId, GetTeamWithoutPlayersDto? ThemTeam, int? MatchQualifyUsTeamId, int? MatchQualifyThemTeamId);
    public record GetMatchWithPlayersDto(int Id, string State, int Level, int TableNumber, int GroupId, Guid? QydhaGameId, Guid? RefereeId, DateTimeOffset StartAt, string? Winner, int? UsTeamId, GetTeamDto? UsTeam, int? ThemTeamId, GetTeamDto? ThemTeam, int? MatchQualifyUsTeamId, int? MatchQualifyThemTeamId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/groups/{group_id}/matches",
            async Task<IResult> (int group_id, HttpContext context, [FromServices] GetGroupMatchesService service) =>
            {
                return (await service.ExecuteAsync(group_id))
                .ResolveToIResult(matches =>
                {
                    var res = new SuccessResponse<List<GetMatchWithoutPlayersDto>>(
                        matches.Select(m => PlayersMapper.MatchToMatchDto(m)).ToList(),
                        "matches fetched successfully.");
                    return TypedResults.Ok(res);
                }, context.TraceIdentifier);
            });
    }

}