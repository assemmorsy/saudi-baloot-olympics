namespace BalootOlympicsTeamsApi.Repositories;
public sealed class ConfirmationRequestRepo(OlympicsContext _dbCtx, OtpManager _otpManager)
{
    public async Task<Result<ConfirmationRequest>> CreateConfirmationRequest(string firstPlayerId, string secondPlayerId)
    {
        var req = new ConfirmationRequest()
        {
            FirstPlayerId = firstPlayerId,
            FirstPlayerOtp = _otpManager.GenerateOTP(),
            SecondPlayerId = secondPlayerId,
            SecondPlayerOtp = _otpManager.GenerateOTP(),
            SentAt = DateTimeOffset.UtcNow,
        };
        _dbCtx.ConfirmationRequests.Add(req);
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(req);
    }
}