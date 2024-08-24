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

    public async Task<Result<List<Team>>> GetAllAsync()
    {
        List<Team> teams = await _dbCtx.Teams
            .Include(t => t.Players)
            .AsSplitQuery()
            .Where(t => t.Players.All(t => t.State == PlayerState.Approved))
            .OrderByDescending(t => t.Id)
            .ToListAsync();
        return Result.Ok(teams);
    }
}