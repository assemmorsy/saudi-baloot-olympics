using BalootOlympicsTeamsApi.Modules.Players;
using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;
using static BalootOlympicsTeamsApi.Modules.Matches.UpdateMatchEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Matches;

public sealed class UpdateMatchService(OlympicsContext _dbCtx)
{
    public async Task<Result<Match>> ExecuteAsync(int matchId, UpdateMatchDataDto dto)
    {
        var match = await _dbCtx
            .Matches
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .AsSplitQuery()
            .AsTracking()
            .SingleOrDefaultAsync(m => m.Id == matchId);
        if (match == null)
            return Result.Fail(new EntityNotFoundError<int>(matchId, nameof(Match)));

        if (!await _dbCtx.Referees.AnyAsync(r => r.Id == dto.RefereeId))
            return Result.Fail(new EntityNotFoundError<Guid>(dto.RefereeId, nameof(Referee)));

        match.RefereeId = dto.RefereeId;
        match.StartAt = dto.StartAt;
        await _dbCtx.Players.AsTracking().Where(p => p.TeamId == match.UsTeamId || p.TeamId == match.ThemTeamId).ToListAsync();
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(match);
    }
}
public sealed class UpdateMatchDataDtoValidator : AbstractValidator<UpdateMatchDataDto>
{
    public UpdateMatchDataDtoValidator()
    {
        RuleFor(x => x.StartAt).NotEmpty();
        RuleFor(x => x.RefereeId).NotEmpty();
    }
}
public sealed class UpdateMatchEndpoint : CarterModule
{
    public sealed record UpdateMatchDataDto(Guid RefereeId, DateTimeOffset StartAt);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/matches/{match_id}",
            async Task<IResult> (int match_id, UpdateMatchDataDto dto, HttpContext context, [FromServices] UpdateMatchService service) =>
            {
                return (await service.ExecuteAsync(match_id, dto))
                .ResolveToIResult(match =>
                {
                    var res = new SuccessResponse<GetMatchWithoutPlayersDto>(
                        PlayersMapper.MatchToMatchDto(match),
                        "match updated successfully.");
                    return TypedResults.Ok(res);
                }, context.TraceIdentifier);
            })
            .AddFluentValidationAutoValidation();

    }

}