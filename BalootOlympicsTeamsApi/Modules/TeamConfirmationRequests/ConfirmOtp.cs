using BalootOlympicsTeamsApi.Modules.Players;
using FluentValidation;
using MediatR;
using static BalootOlympicsTeamsApi.Modules.Teams.GetTeamsEndpoint;

namespace BalootOlympicsTeamsApi.Modules.TeamConfirmationRequests;


public sealed class ConfirmTeamOtpService(PlayerRepo _playerRepo, TeamRepo _teamRepo, OtpManager _otpManager, ConfirmationRequestRepo _confirmationRequestRepo)
{
    public async Task<Result<Team>> ExecuteAsync(ConfirmOtpEndpoint.ConfirmOtpDto dto)
    {
        return (await _confirmationRequestRepo.GetByIdAsync(dto.RequestId))
        .OnSuccess((confirmReq) =>
        {
            if (confirmReq.FirstPlayer.TeamId != null || confirmReq.SecondPlayer.TeamId != null)
                return Result.Fail(new InvalidBodyInputError("احد اللاعبان موجود بفريق بالفعل"));
            if (confirmReq.FirstPlayerOtp != dto.FirstPlayerOtp || confirmReq.SecondPlayerOtp != dto.SecondPlayerOtp)
                return Result.Fail(new InvalidBodyInputError("رمز المرور احد اللاعبان غير صحيح"));
            if (!_otpManager.IsOtpValid(confirmReq.SentAt) || confirmReq.ConfirmedAt != null)
                return Result.Fail(new InvalidBodyInputError("انتهت صلاحية رمز المرور"));
            return Result.Ok(confirmReq);
        })
        .OnSuccessAsync(async (confirmReq) =>
        {
            await _confirmationRequestRepo.UpdateConfirmAt(confirmReq.Id);
            return (await _teamRepo.CreateNewTeam())
                .OnSuccessAsync(async (team) =>
                {
                    return (await _playerRepo.AddPlayersToTeam(team.Id, [confirmReq.FirstPlayerId, confirmReq.SecondPlayerId]))
                        .ToResult((effected) => team);
                });
        })
        .OnSuccessAsync(async (team) => await _teamRepo.GetByIdAsync(team.Id));
    }
}


public sealed class ConfirmOtpDtoValidator : AbstractValidator<ConfirmOtpEndpoint.ConfirmOtpDto>
{
    public ConfirmOtpDtoValidator()
    {
        RuleFor(x => x.FirstPlayerOtp).NotEmpty();
        RuleFor(x => x.SecondPlayerOtp).NotEmpty();
        RuleFor(x => x.RequestId).NotEmpty();
    }
}

public sealed class ConfirmOtpEndpoint : CarterModule
{
    public sealed record ConfirmOtpDto(Guid RequestId, string FirstPlayerOtp, string SecondPlayerOtp);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // app.MapPost("/confirm-team-otp",
        //     async Task<IResult> (ConfirmOtpDto request, HttpContext context, ConfirmTeamOtpService service) =>
        //         (await service.ExecuteAsync(request))
        //         .ResolveToIResult((team) =>
        //             TypedResults.Ok(new SuccessResponse<GetTeamDto>(PlayersMapper.TeamToTeamDto(team)
        //                 , "Team Created successfully.")),
        //                 context.TraceIdentifier))
        //     .WithOpenApi(op =>
        //     {
        //         op.Summary = "endpoint receives Otps and request id then validate the value then add the team";
        //         return op;
        //     })
        //     .AddFluentValidationAutoValidation();

    }
}