using BalootOlympicsTeamsApi.Modules.Players;
namespace BalootOlympicsTeamsApi.Modules.Groups;

public sealed class GenerateGroupsService(GroupRepo _groupRepo, OlympicsContext _dbCtx)
{
    private static List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> GenerateGroupTeamsCount(int teamsCount, int groupsCount, List<DateTime> dates)
    {
        int baseTeamsCountPerGroup = teamsCount / groupsCount;
        int teamsOverCount = teamsCount % groupsCount;
        int dateIndex = 0;
        List<(DateTime PlayAtDate, int groupIndexInThisDate)> datesWithIndex = dates.Select(d => (d, 0)).ToList();
        List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> teamsGroupCount = [];
        for (int i = 0; i < groupsCount; i++)
        {
            int group_count = baseTeamsCountPerGroup;
            if (teamsOverCount > 0)
            {
                group_count++; teamsOverCount--;
            }
            teamsGroupCount.Add((group_count, datesWithIndex[dateIndex].PlayAtDate, datesWithIndex[dateIndex].groupIndexInThisDate));
            datesWithIndex[dateIndex] = (datesWithIndex[dateIndex].PlayAtDate, datesWithIndex[dateIndex].groupIndexInThisDate + 1);
            dateIndex = (dateIndex + 1) % dates.Count;
        }
        return [.. teamsGroupCount.OrderBy(groupData => groupData.PlayAtDate)];
    }
    public async Task<Result<List<Group>>> ExecuteAsync(GenerateGroupsEndpoint.GenerateGroupsRequestDto dto)
    {
        await _dbCtx.Matches.Where(m => true).ExecuteDeleteAsync();
        await _dbCtx.Groups.Where(g => true).ExecuteDeleteAsync();
        List<Team> teams = await _dbCtx.Teams
            .Include(t => t.Players)
            .AsTracking()
            .AsSplitQuery()
            .Where(t => t.Players.All(t => t.State == PlayerState.Approved))
            .OrderByDescending(t => t.Id)
            .ToListAsync();

        List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> teamsGroupCount =
            GenerateGroupTeamsCount(teams.Count, dto.GroupsCount, dto.ChampionshipCheckIn);
        Random rnd = new();
        for (int i = 0; i < teamsGroupCount.Count; i++)
        {
            var (TeamsCount, PlayAtDate, indexOfGroupInThisDate) = teamsGroupCount[i];
            var g = new Group()
            {
                StartPlayAt = PlayAtDate.AddHours(indexOfGroupInThisDate * GenerationConstants.HoursBetweenEachGroup + GenerationConstants.HoursBetweenCheckInAndPlayOfTheGroup),
                CheckInAt = PlayAtDate.AddHours(indexOfGroupInThisDate * GenerationConstants.HoursBetweenEachGroup),
                CompetingTeams = [],
                Name = $"{i + 1} - مجموعة"
            };
            for (int j = 0; j < TeamsCount; j++)
            {
                var teamIndex = rnd.Next(teams.Count);
                g.CompetingTeams.Add(teams[teamIndex]);
                teams.RemoveAt(teamIndex);
            }
            _dbCtx.Groups.Add(g);
        }
        await _dbCtx.SaveChangesAsync();
        return await _groupRepo.GetAsync();
    }
}
public sealed class GenerateGroupsRequestDtoValidator : AbstractValidator<GenerateGroupsEndpoint.GenerateGroupsRequestDto>
{
    public GenerateGroupsRequestDtoValidator()
    {
        RuleFor(x => x.GroupsCount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.ChampionshipCheckIn).Must((list) => list.Count > 0);
        RuleForEach(x => x.ChampionshipCheckIn).GreaterThanOrEqualTo(DateTime.UtcNow.Date);
    }
}
public sealed class GenerateGroupsEndpoint : CarterModule
{
    public record GenerateGroupsRequestDto(int GroupsCount, List<DateTime> ChampionshipCheckIn);
    public record GetGroupDto(int Id, string Name, DateTimeOffset CheckInAt, DateTimeOffset StartPlayAt);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/groups/generate",
            async Task<IResult> (GenerateGroupsRequestDto dto, HttpContext context, [FromServices] GenerateGroupsService service) =>
            {
                return (await service.ExecuteAsync(dto))
                .ResolveToIResult((groups) =>
                {
                    var res = new SuccessResponse<List<GetGroupDto>>(groups.Select(g => PlayersMapper.GroupToGroupDto(g)).ToList(), "Groups Generated Successfully");
                    return Results.Ok(res);
                }, context.TraceIdentifier);
            })
            .AddFluentValidationAutoValidation();
    }

}