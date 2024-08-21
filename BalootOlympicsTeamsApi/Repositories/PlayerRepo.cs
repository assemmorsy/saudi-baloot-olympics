namespace BalootOlympicsTeamsApi.Repositories;
public sealed class PlayerRepo(OlympicsContext _dbCtx)
{
    public async Task<Result<Player>> GetPlayerByPhoneAsync(string phoneNumber)
    {
        Player? player = await _dbCtx.Players
            .Include(p => p.Team)
            .SingleOrDefaultAsync(p => p.Phone == phoneNumber);
        if (player == null) return Result.Fail(new EntityNotFoundError<string>(phoneNumber, nameof(Player)));
        return Result.Ok(player);
    }
}