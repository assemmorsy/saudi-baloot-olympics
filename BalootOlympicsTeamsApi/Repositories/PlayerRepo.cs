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
    public async Task<Result<Player>> GetPlayerByIdAsync(string id)
    {
        Player? player = await _dbCtx.Players
            .Include(p => p.Team)
            .SingleOrDefaultAsync(p => p.Id == id);
        if (player == null) return Result.Fail(new EntityNotFoundError<string>(id, nameof(Player)));
        return Result.Ok(player);
    }
    public async Task<Result<Player>> GetPlayerByEmailAsync(string email)
    {
        Player? player = await _dbCtx.Players
            .Include(p => p.Team)
            .SingleOrDefaultAsync(p => p.Email == email);
        if (player == null) return Result.Fail(new EntityNotFoundError<string>(email, nameof(Player)));
        return Result.Ok(player);
    }
}