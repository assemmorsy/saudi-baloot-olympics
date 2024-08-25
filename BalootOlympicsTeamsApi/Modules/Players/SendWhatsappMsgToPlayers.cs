namespace BalootOlympicsTeamsApi.Modules.Players;

public sealed class SendWhatsappMsgToPlayersService(PlayerRepo _playerRepo, WhatsAppService _whatsAppService)
{
    public async Task<Result> ExecuteAsync() =>
        (await _playerRepo.GetApprovedPlayersWithoutTeams())
        .OnSuccessAsync(async players =>
        {
            foreach (var p in players)
            {
                await _whatsAppService.SendMessageFromTemplateAsync(p.Phone, "request_join_team");
            }
            return Result.Ok();
        });

}
public sealed class SendWhatsappMsgToPlayersEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/players/send-whatsapp-alert-to-join-teams/",
            async Task<IResult> (HttpContext context, [FromServices] SendWhatsappMsgToPlayersService service) =>
            {
                await service.ExecuteAsync();
                return Results.Ok("Done");
            });
    }

}