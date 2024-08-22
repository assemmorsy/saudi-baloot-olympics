namespace BalootOlympicsTeamsApi.Repositories;
public sealed class TeamRepo(OlympicsContext _dbCtx)
{
    public async Task<Result<Team>> CreateNewTeam()
    {
        var newTeam = new Team();
        _dbCtx.Teams.Add(newTeam);
        await _dbCtx.SaveChangesAsync();
        return Result.Ok(newTeam);
    }
    public async Task<Result<Team>> GetByIdAsync(int id)
    {
        Team? team = await _dbCtx.Teams
            .Include(t => t.Players)
            .SingleOrDefaultAsync(t => t.Id == id);
        if (team == null) return Result.Fail(new EntityNotFoundError<int>(id, nameof(Team)));
        return Result.Ok(team);
    }

}