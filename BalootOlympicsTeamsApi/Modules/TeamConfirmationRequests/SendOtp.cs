using FluentValidation;
using MediatR;

namespace BalootOlympicsTeamsApi.Modules.TeamConfirmationRequests;


public sealed class SendConfirmTeamOtpService(PlayerRepo _playerRepo, ConfirmationRequestRepo _confirmationRequestRepo, IMediator _mediator)
{
    public async Task<Result<ConfirmationRequest>> ExecuteAsync(SendOtpEndpoint.SendOtpDto dto)
    {
        if (dto.FirstPlayerId == dto.SecondPlayerId)
            return Result.Fail(new InvalidBodyInputError("يجب ادخال ارقام مرجعية مختلفة لكل لاعب"));

        var firstPlayerRes = await _playerRepo.GetPlayerByAsync(p => p.Id == dto.FirstPlayerId, dto.FirstPlayerId);
        if (firstPlayerRes.IsFailed)
            return Result.Fail(new InvalidBodyInputError("الرقم المرجعى للاعب الاول غير موجود"));

        var secondPlayerRes = await _playerRepo.GetPlayerByAsync(p => p.Id == dto.SecondPlayerId, dto.SecondPlayerId);
        if (secondPlayerRes.IsFailed)
            return Result.Fail(new InvalidBodyInputError("الرقم المرجعى للاعب الثانى غير موجود"));

        var firstPlayer = firstPlayerRes.Value;
        var secondPlayer = secondPlayerRes.Value;

        if (firstPlayer.TeamId != null || secondPlayer.TeamId != null)
            return Result.Fail(new InvalidBodyInputError("احد اللاعبان موجود بفريق بالفعل"));

        if (firstPlayer.State != PlayerState.Approved || secondPlayer.State != PlayerState.Approved)
            return Result.Fail(new InvalidBodyInputError("احد اللاعبان لم يتم قبوله اما لعدم استيفاء شروط البطولة او عدم اكمال البيانات "));

        return (await _confirmationRequestRepo.CreateConfirmationRequest(firstPlayer.Id, secondPlayer.Id))
            .OnSuccess((confirmReq) =>
            {
                _mediator.Publish(new SendOtpNotification(firstPlayer, confirmReq.FirstPlayerOtp));
                _mediator.Publish(new SendOtpNotification(secondPlayer, confirmReq.SecondPlayerOtp));
                return Result.Ok(confirmReq);
            });
    }

    public sealed class SendOtpNotification(Player player, string otp) : INotification
    {
        public Player Player { get; } = player;
        public string Otp { get; } = otp;
    }
    public sealed class SendOtpHandler(WhatsAppService whatsAppService, ZohoMailingService zohoMailingService) : INotificationHandler<SendOtpNotification>
    {
        private readonly WhatsAppService _whatsAppService = whatsAppService;
        private readonly ZohoMailingService _zohoMailingService = zohoMailingService;
        public async Task Handle(SendOtpNotification notification, CancellationToken cancellationToken)
        {
            await _whatsAppService.SendOtpAsync(notification.Player.Phone, notification.Player.NameAr, notification.Otp);
            await _zohoMailingService.SendOtpToEmailAsync(notification.Player.Email, notification.Otp);
        }
    }
}


public sealed class SendOtpDtoValidator : AbstractValidator<SendOtpEndpoint.SendOtpDto>
{
    public SendOtpDtoValidator()
    {
        RuleFor(x => x.FirstPlayerId).NotEmpty();
        RuleFor(x => x.SecondPlayerId).NotEmpty();
    }
}

public sealed class SendOtpEndpoint : CarterModule
{
    public sealed record ResponseDto(Guid RequestId);
    public sealed record SendOtpDto(string FirstPlayerId, string SecondPlayerId);
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/send-team-confirmation-otp",
            IResult (SendOtpDto request, HttpContext context, SendConfirmTeamOtpService service) =>
            {
                return TypedResults.BadRequest(new { message = "تم اغلاق باب التقديم " });
            })
            .WithOpenApi(op =>
            {
                op.Summary = "Send confirmation to the provided phones numbers";
                op.Description = "this will check for this phone number existence then if these players not joined to a team it will send confirmation to the email and the whatsapp number.";
                return op;
            })
            .AddFluentValidationAutoValidation();

    }
}