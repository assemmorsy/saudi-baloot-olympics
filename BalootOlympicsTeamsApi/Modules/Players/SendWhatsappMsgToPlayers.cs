namespace BalootOlympicsTeamsApi.Modules.Players;

public sealed class SendWhatsappMsgToPlayersService(OlympicsContext _dbCtx, WhatsAppService _whatsAppService)
{
    public async Task<Result> ExecuteAsync()
    {
        var teams = await _dbCtx.Teams.Include(t => t.Players).Where(t => t.GroupId != null).ToListAsync();
        foreach (var t in teams)
        {
            foreach (var p in t.Players)
            {
                await _whatsAppService.SendMessageFromTemplateAsync(p.Phone, "approved_teams_reminder");
            }
        }
        return Result.Ok();
    }
}
// public sealed class SendWhatsappMsgToPlayersEndpoint : CarterModule
// {
//     public override void AddRoutes(IEndpointRouteBuilder app)
//     {
//         app.MapPost("/players/send-whatsapp-alert-to-join-teams/",
//             async Task<IResult> (HttpContext context, [FromServices] SendWhatsappMsgToPlayersService service) =>
//             {
//                 await service.ExecuteAsync();
//                 return Results.Ok("Done");
//             });
//     }
// }