
namespace BalootOlympicsTeamsApi.Repositories;
public sealed class GroupRepo(OlympicsContext _dbCtx)
{

    public async Task<Result<Group>> GetByIdAsync(int groupId)
    {
        var group = await _dbCtx.Groups
            .SingleOrDefaultAsync(g => g.Id == groupId);
        return group == null ?
            Result.Fail(new EntityNotFoundError<int>(groupId, nameof(Group))) :
            Result.Ok(group);
    }
    public async Task<Result<List<Group>>> GetAsync()
    {
        var groups = await _dbCtx.Groups.Include(g => g.CompetingTeams).ThenInclude(t => t.Players).AsSplitQuery().OrderBy(g => g.StartPlayAt).ToListAsync();
        return Result.Ok(groups);
    }
    public async Task<Result<int>> DeleteAllAsync()
    {
        var effected = await _dbCtx.Groups.Where(g => true).ExecuteDeleteAsync();
        return Result.Ok(effected);
    }

}