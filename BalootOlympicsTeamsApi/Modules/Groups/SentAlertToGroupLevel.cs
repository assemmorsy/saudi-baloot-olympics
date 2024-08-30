using BalootOlympicsTeamsApi.Modules.Players;

namespace BalootOlympicsTeamsApi.Modules.Groups;

public sealed class SendAlertToGroupLevelService(OlympicsContext _dbCtx, WhatsAppService _whatsAppService)
{
    public async Task<Result> ExecuteAsync(int groupId, int level)
    {
        List<int> teamsIds = [];
        var matches = await _dbCtx.Matches
            .Where(m => m.GroupId == groupId && m.Level == level && m.UsTeamId != null && m.ThemTeamId != null)
            .ToListAsync();
        matches.ForEach(m =>
        {
            teamsIds.Add(m.UsTeamId!.Value);
            teamsIds.Add(m.ThemTeamId!.Value);
        });
        var teams = await _dbCtx.Teams.Include(t => t.Players).Where(t => teamsIds.Contains(t.Id)).ToListAsync();
        foreach (var t in teams)
        {
            foreach (var p in t.Players)
            {
                await _whatsAppService.SendMessageFromTemplateAsync(p.Phone, "game_start_soon_alert");
            }
        }
        return Result.Ok();
    }
}

public sealed class SendAlertToGroupLevelEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/groups/{groupId}/send-game-alert/{level}",
            async Task<IResult> (int groupId, int level, HttpContext context, [FromServices] SendAlertToGroupLevelService service) =>
            {
                return (await service.ExecuteAsync(groupId, level))
                    .ResolveToIResult(() => Results.Ok("Done"), context.TraceIdentifier);
            });
    }
}