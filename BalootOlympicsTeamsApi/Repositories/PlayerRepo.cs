using System.Linq.Expressions;

namespace BalootOlympicsTeamsApi.Repositories;
public sealed class PlayerRepo(OlympicsContext _dbCtx)
{
    public async Task<Result<Player>> GetPlayerByAsync<T>(Expression<Func<Player, bool>> predicate, T identifier)
    {
        Player? player = await _dbCtx.Players
            .Include(p => p.Team)
            .SingleOrDefaultAsync(predicate);
        if (player == null) return Result.Fail(new EntityNotFoundError<T>(identifier, nameof(Player)));
        return Result.Ok(player);
    }

    public async Task<Result<Player>> AddPlayersToTeam(int teamId, List<string> playerIds)
    {
        var affected = await _dbCtx.Players.Where(p => playerIds.Contains(p.Id))
        .ExecuteUpdateAsync(
            setters => setters
                .SetProperty(p => p.TeamId, teamId)
        );
        return affected == 2 ?
            Result.Ok() :
            Result.Fail(new EntityNotFoundError<int>(teamId, nameof(Team)));
    }
}