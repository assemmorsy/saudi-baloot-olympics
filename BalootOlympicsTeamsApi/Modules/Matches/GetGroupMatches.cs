using BalootOlympicsTeamsApi.Modules.Players;
using static BalootOlympicsTeamsApi.Modules.Teams.GetTeamsEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Matches;

public sealed class GetGroupMatchesService(OlympicsContext _dbCtx)
{
    public async Task<Result<(List<Match> Matches, Group Group)>> ExecuteAsync(int groupId)
    {
        var group = await _dbCtx.Groups
            .Include(g => g.CompetingTeams)
            .ThenInclude(t => t.Players)
            .AsSplitQuery()
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
        return Result.Ok((matches, group));
    }
}

public sealed class GetGroupMatchesEndpoint : CarterModule
{
    public record GetGroupMatchesRequestDto(int QualifiedTeamsCount);
    public record GetMatchDto(int Id, string State, int Level, int TableNumber, int GroupId, Guid? QydhaGameId, Guid? RefereeId, DateTimeOffset StartAt, string? Winner, int? UsTeamId, int? ThemTeamId, int? MatchQualifyUsTeamId, int? MatchQualifyThemTeamId);
    public record GetMatchesResponseDto(List<GetMatchDto> Matches, List<GetTeamWithoutPlayersDto> Teams);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/groups/{group_id}/matches",
            async Task<IResult> (int group_id, HttpContext context, [FromServices] GetGroupMatchesService service) =>
            {
                return (await service.ExecuteAsync(group_id))
                .ResolveToIResult(data =>
                {
                    var res = new SuccessResponse<GetMatchesResponseDto>(
                        new GetMatchesResponseDto(
                             data.Matches.Select(m => PlayersMapper.MatchToMatchDto(m)).ToList(),
                             data.Group.CompetingTeams.Select(t => PlayersMapper.TeamToTeamWithoutPlayersDto(t)).ToList()
                        ),
                        "matches fetched successfully.");
                    return TypedResults.Ok(res);
                }, context.TraceIdentifier);
            });
    }

}