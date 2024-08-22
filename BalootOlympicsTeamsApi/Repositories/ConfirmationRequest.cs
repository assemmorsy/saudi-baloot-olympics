namespace BalootOlympicsTeamsApi.Repositories;
public sealed class ConfirmationRequestRepo(OlympicsContext _dbCtx, OtpManager _otpManager)
{
    public async Task<Result<ConfirmationRequest>> CreateConfirmationRequest(string firstPlayerId, string secondPlayerId)
    {
        var req = new ConfirmationRequest()
        {
            FirstPlayerId = firstPlayerId,
            FirstPlayerOtp = _otpManager.GenerateOTP(DateTime.Now),
            SecondPlayerId = secondPlayerId,
            SecondPlayerOtp = _otpManager.GenerateOTP(DateTime.Now.AddMinutes(5)),
            SentAt = DateTimeOffset.UtcNow,
        };
        _dbCtx.ConfirmationRequests.Add(req);
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(req);
    }
    public async Task<Result<ConfirmationRequest>> GetByIdAsync(Guid id)
    {
        ConfirmationRequest? confirmReq = await _dbCtx.ConfirmationRequests
            .Include(req => req.FirstPlayer)
            .Include(req => req.SecondPlayer)
            .SingleOrDefaultAsync(p => p.Id == id);
        if (confirmReq == null) return Result.Fail(new EntityNotFoundError<Guid>(id, nameof(ConfirmationRequest)));
        return Result.Ok(confirmReq);
    }

    public async Task<Result> UpdateConfirmAt(Guid id)
    {
        var affected = await _dbCtx.ConfirmationRequests.Where(req => req.Id == id)
        .ExecuteUpdateAsync(
            setters => setters
                .SetProperty(req => req.ConfirmedAt, DateTimeOffset.UtcNow)
        );
        return affected == 1 ?
            Result.Ok() :
            Result.Fail(new EntityNotFoundError<Guid>(id, nameof(ConfirmationRequest)));
    }
}