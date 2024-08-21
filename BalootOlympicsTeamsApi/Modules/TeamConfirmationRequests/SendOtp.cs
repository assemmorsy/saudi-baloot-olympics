// using FluentValidation;
// using MediatR;

// namespace BalootOlympicsTeamsApi.Modules.TeamConfirmationRequests;

// public static class SendOtp
// {
//     // OlympicsContext _dbCtx
//     public sealed class Service(PlayerRepo _playerRepo, ConfirmationRequestRepo _confirmationRequestRepo, IMediator _mediator)
//         : IAsyncCommandService<SendOtpEndpoint.SendOtpDto, Result<ConfirmationRequest>>
//     {
//         public async Task<Result<ConfirmationRequest>> ExecuteAsync(SendOtpEndpoint.SendOtpDto dto)
//         {
//             var firstPlayerRes = await _playerRepo.GetPlayerByPhoneAsync(dto.FirstPlayerPhone);
//             var secondPlayerRes = await _playerRepo.GetPlayerByPhoneAsync(dto.SecondPlayerPhone);
//             if (firstPlayerRes.IsFailed || secondPlayerRes.IsFailed)
//                 return firstPlayerRes.Merge(secondPlayerRes);
//             var firstPlayer = firstPlayerRes.Value;
//             var secondPlayer = secondPlayerRes.Value;

//             if (firstPlayer.TeamId == null && secondPlayer.TeamId == null)
//             {
//                 return (await _confirmationRequestRepo.CreateConfirmationRequest(firstPlayer.Id, secondPlayer.Id))
//                 .OnSuccessAsync((confirmReq) =>
//                 {
//                     _mediator.Publish(new SendOtpNotification(firstPlayer, confirmReq.FirstPlayerOtp));
//                     _mediator.Publish(new SendOtpNotification(secondPlayer, confirmReq.SecondPlayerOtp));
//                     return Result.Ok(confirmReq);
//                 });
//             }
//             else if (firstPlayer.TeamId != null && secondPlayer.TeamId != null && firstPlayer.TeamId == secondPlayer.TeamId)
//             {
//                 // return success and there team data
//             }
//             else
//             {
//                 return Result.Fail(new InvalidBodyInputError("one of the players already registered in a team or the players registered in two different teams "));
//             }

//         }
//     }

//     public sealed class SendOtpNotification(Player player, string otp) : INotification
//     {
//         public Player Player { get; } = player;
//         public string Otp { get; } = otp;
//     }
//     public sealed class SendOtpHandler(WhatsAppService whatsAppService, MailingService mailingService) : INotificationHandler<SendOtpNotification>
//     {
//         private readonly WhatsAppService _whatsAppService = whatsAppService;
//         private readonly MailingService _mailingService = mailingService;
//         public async Task Handle(SendOtpNotification notification, CancellationToken cancellationToken)
//         {
//             await _whatsAppService.SendOtpAsync(notification.Player.Phone, notification.Player.Name, notification.Otp);
//             await _mailingService.SendOtpToEmailAsync(notification.Player.Email, notification.Otp);
//         }
//     }
// }


// public sealed class SendOtpDtoValidator : AbstractValidator<SendOtpEndpoint.SendOtpDto>
// {
//     public SendOtpDtoValidator()
//     {
//         RuleFor(x => x.FirstPlayerPhone).NotEmpty();
//         RuleFor(x => x.SecondPlayerPhone).NotEmpty();
//     }
// }

// public sealed class SendOtpEndpoint : CarterModule
// {
//     public sealed record SendOtpDto(string FirstPlayerPhone, string SecondPlayerPhone);
//     public override void AddRoutes(IEndpointRouteBuilder app)
//     {
//         app.MapPost("/send-team-otp",
//             IResult (SendOtpDto request, IAsyncCommandService<SendOtpDto, Result<ConfirmationRequest>> service) =>
//             {
//                 return TypedResults.Ok(new { message = "done" });
//             })
//             .WithOpenApi(op =>
//             {
//                 op.Summary = "Send confirmation to the provided phones numbers";
//                 op.Description = "this will check for this phone number existence then if these players not joined to a team it will send confirmation to the email and the whatsapp number.";
//                 return op;
//             });
//     }
// }