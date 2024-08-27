using BalootOlympicsTeamsApi.Extensions;
using BalootOlympicsTeamsApi.Modules.Players;

using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;
namespace BalootOlympicsTeamsApi.Modules.Matches;

public sealed class GenerateGroupMatchesService(OlympicsContext _dbCtx)
{
    public static bool IsPowerOf2(int num)
    {
        if (num <= 0)
            return false;
        return (num & (num - 1)) == 0;
    }
    private static Match? CreateChildMatches(Group group, int currentLevel, List<int> teamsIds)
    {
        if (teamsIds.Count == 1) return null;
        else if (teamsIds.Count == 2)
            return new Match()
            {
                Level = currentLevel,
                GroupId = group.Id,
                TableNumber = 0,
                State = MatchState.Created,
                StartAt = group.StartPlayAt,
                ThemTeamId = teamsIds[0],
                UsTeamId = teamsIds[1],
            };
        else if (teamsIds.Count == 3)
            return new Match()
            {
                Level = currentLevel,
                GroupId = group.Id,
                TableNumber = 0,
                State = MatchState.Created,
                StartAt = group.StartPlayAt,
                ThemTeamId = teamsIds[0],
                MatchQualifyUsTeam = CreateChildMatches(group, currentLevel + 1, teamsIds.Skip(1).ToList())
            };
        else
        {
            int middle = (int)Math.Floor(teamsIds.Count / 2.0);
            return new Match()
            {
                Level = currentLevel,
                GroupId = group.Id,
                TableNumber = 0,
                State = MatchState.Created,
                StartAt = group.StartPlayAt,
                MatchQualifyThemTeam = CreateChildMatches(group, currentLevel + 1, teamsIds.Take(middle).ToList()),
                MatchQualifyUsTeam = CreateChildMatches(group, currentLevel + 1, teamsIds.Skip(middle).ToList())
            };
        }
    }
    public async Task<Result<List<Match>>> ExecuteAsync(int groupId, GenerateGroupMatchesEndpoint.GenerateGroupMatchesRequestDto dto)
    {
        var group = await _dbCtx.Groups
           .Include(g => g.CompetingTeams.OrderBy(t => t.Id))
           .AsTracking()
           .AsSplitQuery()
           .SingleOrDefaultAsync(g => g.Id == groupId);
        await _dbCtx.Matches.Where(m => m.GroupId == groupId).ExecuteDeleteAsync();
        if (group == null) return Result.Fail(new EntityNotFoundError<int>(groupId, nameof(Group)));


        List<(int TeamsCount, List<int> TeamsIds)> branchesData = CreateBranchesData(group.CompetingTeams, dto.QualifiedTeamsCount);

        branchesData.ForEach(brachData =>
        {
            var m = CreateChildMatches(group, 1, brachData.TeamsIds);
            if (m != null)
                _dbCtx.Matches.Add(m);
        });

        await _dbCtx.SaveChangesAsync();

        var matches = await _dbCtx.Matches
            .Where(m => m.GroupId == groupId)
            .AsTracking()
            .OrderByDescending(m => m.Level)
            .ToListAsync();
        int maxLevel = matches[0].Level;
        for (int i = 0; i < matches.Count; i++)
        {
            matches[i].StartAt = group.StartPlayAt.AddHours(GenerationConstants.MatchDurationInHours * (maxLevel - matches[i].Level));
            matches[i].TableNumber = i + 1;
        }
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(matches);
    }
    private static List<(int TeamsCount, List<int> TeamsIds)> CreateBranchesData(List<Team> teams, int qualifiedTeamsCount)
    {
        teams.Shuffle();
        int branchBaseTeamsCount = teams.Count / qualifiedTeamsCount;
        int teamsOverCount = teams.Count % qualifiedTeamsCount;

        List<(int TeamsCount, List<int> TeamsIds)> branchesTeamsCount = [];
        for (int i = 0; i < qualifiedTeamsCount; i++)
        {
            int teamsCount = branchBaseTeamsCount;
            if (teamsOverCount > 0)
                teamsCount++; teamsOverCount--;
            branchesTeamsCount.Add((teamsCount, []));
        }

        int totalTeamsIndex = 0;
        for (int i = 0; i < qualifiedTeamsCount; i++)
        {
            branchesTeamsCount[i] = (
                branchesTeamsCount[i].TeamsCount,
                teams.Slice(totalTeamsIndex, branchesTeamsCount[i].TeamsCount).Select(t => t.Id).ToList());
            totalTeamsIndex += branchesTeamsCount[i].TeamsCount;
        }
        return branchesTeamsCount;
    }
}
public sealed class GenerateGroupMatchesRequestDtoValidator : AbstractValidator<GenerateGroupMatchesEndpoint.GenerateGroupMatchesRequestDto>
{
    public GenerateGroupMatchesRequestDtoValidator()
    {
        RuleFor(x => x.QualifiedTeamsCount).GreaterThan(0)
            .Must(GenerateGroupMatchesService.IsPowerOf2)
            .WithMessage("QualifiedTeamsCount Must be number of power of 2.");

    }
}
public sealed class GenerateGroupMatchesEndpoint : CarterModule
{
    public record GenerateGroupMatchesRequestDto(int QualifiedTeamsCount);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/groups/{group_id}/generate-matches",
            async Task<IResult> (int group_id, GenerateGroupMatchesRequestDto dto, HttpContext context, [FromServices] GenerateGroupMatchesService service) =>
            {
                return (await service.ExecuteAsync(group_id, dto))
                .ResolveToIResult((matches) =>
                {
                    var res = new SuccessResponse<List<GetMatchWithoutPlayersDto>>(
                        matches.Select(m => PlayersMapper.MatchToMatchDto(m)).ToList(),
                        "matches fetched successfully.");
                    return TypedResults.Ok(res);
                }, context.TraceIdentifier);
            })
            .AddFluentValidationAutoValidation();

    }

}