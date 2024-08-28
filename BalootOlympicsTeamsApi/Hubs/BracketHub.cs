using System.Text.Json;
using BalootOlympicsTeamsApi.Modules.Players;
using Microsoft.AspNetCore.SignalR;
using static BalootOlympicsTeamsApi.Modules.Matches.GetGroupMatchesEndpoint;

namespace BalootOlympicsTeamsApi.Hubs;

public interface IBracketClient
{
    Task BracketChanged(string groupId, string groupData);
}
public class BracketHub(OlympicsContext _dbCtx) : Hub<IBracketClient>
{
    public async Task<string> GetGroupBracket(int groupId)
    {
        var group = await _dbCtx.Groups
                    .Include(g => g.CompetingTeams)
                    .ThenInclude(t => t.Players)
                    .AsSplitQuery()
                    .AsTracking()
                    .SingleOrDefaultAsync(g => g.Id == groupId);
        if (group == null)
            return JsonSerializer.Serialize(new EntityNotFoundError<int>(groupId, nameof(Group)).ToErrorResponse(string.Empty));

        var matches = await _dbCtx.Matches
            .Where(m => m.GroupId == groupId)
            .Include(m => m.UsTeam)
            .Include(m => m.ThemTeam)
            .AsSplitQuery()
            .AsTracking()
            .OrderByDescending(m => m.Level).ThenBy(m => m.TableNumber)
            .ToListAsync();
        var res = new SuccessResponse<List<GetMatchWithoutPlayersDto>>(
            matches.Select(m => PlayersMapper.MatchToMatchDto(m)).ToList(),
            "matches fetched successfully.");
        return JsonSerializer.Serialize(res);

    }

}