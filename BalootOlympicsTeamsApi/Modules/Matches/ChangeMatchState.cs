using System.Text.Json;
using BalootOlympicsTeamsApi.Modules.Players;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using static BalootOlympicsTeamsApi.Modules.Matches.ChangeMatchStateEndpoint;
using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;

namespace BalootOlympicsTeamsApi.Modules.Matches;

public sealed class ChangeMatchStateService(OlympicsContext _dbCtx)
{
    public async Task<Result<Match>> StartMatchAsync(int matchId, StartMatchDto dto)
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
        if (match.UsTeamId == null || match.ThemTeamId == null)
            return Result.Fail(new InvalidBodyInputError("لا يمكن بدأ مباراة لم يحدد طرفيها بعد."));
        if (match.State != MatchState.Created)
            return Result.Fail(new InvalidBodyInputError("لا يمكن بدأ مباراة انتهت او جارية."));

        match.State = MatchState.Running;
        match.QydhaGameId = dto.GameId;
        await _dbCtx.Players.AsTracking().Where(p => p.TeamId == match.UsTeamId || p.TeamId == match.ThemTeamId).ToListAsync();
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(match);
    }

    public async Task<Result<Match>> EndMatchAsync(int matchId, EndMatchDto dto)
    {
        var match = await _dbCtx.Matches
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .Include(m => m.MatchQualifyThemTeam)
            .Include(m => m.MatchQualifyUsTeam)
            .AsSplitQuery()
            .AsTracking()
            .SingleOrDefaultAsync(m => m.Id == matchId);
        if (match == null)
            return Result.Fail(new EntityNotFoundError<int>(matchId, nameof(Match)));
        if (match.State == MatchState.Ended)
            return Result.Fail(new InvalidBodyInputError("لا يمكن انهاء مباراة انتهت بالفعل"));
        if (match.State == MatchState.Running && dto.Winner == null)
            return Result.Fail(new InvalidBodyInputError("لا يمكن انهاء مباراة تلعب بدون ارفاق فائز"));

        if ((match.UsTeamId == null && match.MatchQualifyUsTeam != null && match.MatchQualifyUsTeam.State != MatchState.Ended) ||
            (match.ThemTeamId == null && match.MatchQualifyThemTeam != null && match.MatchQualifyThemTeam.State != MatchState.Ended))
            return Result.Fail(new InvalidBodyInputError("لا يمكن بدأ مباراة لم يحدد طرفيها بعد"));

        match.State = MatchState.Ended;
        match.Winner = dto.Winner;
        await _dbCtx.Players.AsTracking().Where(p => p.TeamId == match.UsTeamId || p.TeamId == match.ThemTeamId).ToListAsync();
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(match);
    }


    private int? TraverseMatchesTree(Match? match)
    {
        if (match == null) return null;
        int? themQualifiedTeamId, usQualifiedTeamId;
        _dbCtx.Entry(match).Reference(m => m.MatchQualifyThemTeam).Load();
        themQualifiedTeamId = TraverseMatchesTree(match.MatchQualifyThemTeam);
        if (themQualifiedTeamId != null) match.ThemTeamId = themQualifiedTeamId.Value;

        _dbCtx.Entry(match).Reference(m => m.MatchQualifyUsTeam).Load();
        usQualifiedTeamId = TraverseMatchesTree(match.MatchQualifyUsTeam);
        if (usQualifiedTeamId != null) match.UsTeamId = usQualifiedTeamId.Value;

        bool bothTeamsWithdrawInThemQualifiedMatch = match.MatchQualifyThemTeamId != null && match.MatchQualifyThemTeam != null && match.MatchQualifyThemTeam.State == MatchState.Ended && match.MatchQualifyThemTeam.Winner == null;
        bool bothTeamsWithdrawInUsQualifiedMatch = match.MatchQualifyUsTeamId != null && match.MatchQualifyUsTeam != null && match.MatchQualifyUsTeam.State == MatchState.Ended && match.MatchQualifyUsTeam.Winner == null;

        if (bothTeamsWithdrawInThemQualifiedMatch || bothTeamsWithdrawInUsQualifiedMatch)
        {
            if (bothTeamsWithdrawInThemQualifiedMatch && bothTeamsWithdrawInUsQualifiedMatch)
            {
                match.State = MatchState.Ended;
                match.Winner = null;
                return null;
            }
            else if (bothTeamsWithdrawInThemQualifiedMatch)
            {
                if (match.MatchQualifyUsTeam != null && match.MatchQualifyUsTeam.State != MatchState.Ended)
                {
                    return null;
                }
                match.State = MatchState.Ended;
                match.Winner = MatchSide.Us;
                return match.UsTeamId;
            }
            else
            {
                if (match.MatchQualifyThemTeam != null && match.MatchQualifyThemTeam.State != MatchState.Ended)
                {
                    return null;
                }
                match.State = MatchState.Ended;
                match.Winner = MatchSide.Them;
                return match.ThemTeamId;
            }
        }

        if (match.State != MatchState.Ended) return null;

        if (match.UsTeamId != null && match.ThemTeamId != null)
        {
            if (match.Winner != null)
                return match.Winner == MatchSide.Us ? match.UsTeamId : match.ThemTeamId;
            else
                return null;
        }
        throw new Exception("condition not covered in TraverseMatchesTree function");
    }

    public async Task UpdateGroupBracket(int groupId)
    {
        var bracketHeads = await _dbCtx.Matches
            // .Include(m => m.MatchQualifyThemTeam)
            // .Include(m => m.MatchQualifyUsTeam)
            .AsSplitQuery().AsTracking()
            .Where(m => m.GroupId == groupId && m.Level == 1).ToListAsync();

        bracketHeads.ForEach(m =>
        {
            TraverseMatchesTree(m);
        });

        await _dbCtx.SaveChangesAsync();
    }
}
public sealed class StartMatchDtoValidator : AbstractValidator<StartMatchDto>
{
    public StartMatchDtoValidator()
    {
        RuleFor(x => x.GameId).NotEmpty();
    }
}
public sealed class EndMatchDtoValidator : AbstractValidator<EndMatchDto>
{
    public EndMatchDtoValidator()
    {
        When(x => x.Winner != null, () =>
        {
            RuleFor(x => x.Winner).IsInEnum();
        });
    }
}


public sealed class GroupBracketChangedNotification(int groupId) : INotification
{
    public int GroupId { get; } = groupId;
}
public sealed class SendOtpHandler(IHubContext<BracketHub, IBracketClient> hubContext, OlympicsContext _dbCtx) : INotificationHandler<GroupBracketChangedNotification>
{
    public async Task Handle(GroupBracketChangedNotification notification, CancellationToken cancellationToken)
    {
        var group = await _dbCtx.Groups
                          .Include(g => g.CompetingTeams)
                          .ThenInclude(t => t.Players)
                          .AsSplitQuery()
                          .AsTracking()
                          .SingleOrDefaultAsync(g => g.Id == notification.GroupId);

        if (group == null)
            return;
        // JsonSerializer.Serialize(new EntityNotFoundError<int>(notification.GroupId, nameof(Group)).ToErrorResponse(string.Empty));

        var matches = await _dbCtx.Matches
            .Where(m => m.GroupId == notification.GroupId)
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .AsSplitQuery()
            .AsTracking()
            .OrderByDescending(m => m.Level).ThenBy(m => m.TableNumber)
            .ToListAsync();
        var res = new SuccessResponse<List<GetMatchWithoutPlayersDto>>(
            matches.Select(m => PlayersMapper.MatchToMatchDto(m)).ToList(),
            "matches fetched successfully.");

        await hubContext.Clients.All.BracketChanged(notification.GroupId.ToString(), JsonSerializer.Serialize(res, SerializationConstants.SerializerOptions));
    }
}


public sealed class ChangeMatchStateEndpoint : CarterModule
{
    public sealed record StartMatchDto(Guid GameId);
    public sealed record EndMatchDto(MatchSide? Winner);

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // app.MapPost("/matches/{match_id}/start",
        //     async Task<IResult> (int match_id, StartMatchDto dto, HttpContext context, IMediator _mediator, [FromServices] ChangeMatchStateService service) =>
        //     {
        //         return (await service.StartMatchAsync(match_id, dto))
        //             .OnSuccessAsync(async (match) =>
        //             {
        //                 await _mediator.Publish(new GroupBracketChangedNotification(match.GroupId));
        //                 return Result.Ok(match);
        //             })
        //             .ResolveToIResult(match =>
        //             {
        //                 var res = new SuccessResponse<GetMatchWithoutPlayersDto>(
        //                     PlayersMapper.MatchToMatchDto(match),
        //                     "match Started successfully.");
        //                 return TypedResults.Ok(res);
        //             }, context.TraceIdentifier);
        //     })
        //     .AddFluentValidationAutoValidation();

        // app.MapPost("/matches/{match_id}/end",
        //     async Task<IResult> (int match_id, EndMatchDto dto, HttpContext context, IMediator _mediator, [FromServices] ChangeMatchStateService service) =>
        //     {
        //         return (await service.EndMatchAsync(match_id, dto))
        //         .OnSuccessAsync(async match =>
        //         {
        //             await service.UpdateGroupBracket(match.GroupId);
        //             await _mediator.Publish(new GroupBracketChangedNotification(match.GroupId));
        //             return Result.Ok(match);
        //         })
        //         .ResolveToIResult(match =>
        //         {
        //             var res = new SuccessResponse<GetMatchWithoutPlayersDto>(
        //                 PlayersMapper.MatchToMatchDto(match),
        //                 "match Ended successfully.");
        //             return TypedResults.Ok(res);
        //         }, context.TraceIdentifier);
        //     })
        //     .AddFluentValidationAutoValidation();

        // app.MapPut("/matches/{match_id}/reset",
        //     async Task<IResult> (int match_id, HttpContext context, [FromServices] ChangeMatchStateService service) =>
        //     {

        //     })
        //     .AddFluentValidationAutoValidation();
    }

}