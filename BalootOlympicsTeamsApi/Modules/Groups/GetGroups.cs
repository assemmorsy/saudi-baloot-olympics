using BalootOlympicsTeamsApi.Modules.Players;

namespace BalootOlympicsTeamsApi.Modules.Groups;

public sealed class GetGroupsService(GroupRepo _groupRepo)
{
    public async Task<Result<List<Group>>> ExecuteAsync() => await _groupRepo.GetAsync();
}

public sealed class GetGroupsEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/groups",
            async Task<IResult> (HttpContext context, [FromServices] GetGroupsService service) =>
            {
                return (await service.ExecuteAsync())
                .ResolveToIResult((groups) =>
                {
                    var res = new SuccessResponse<List<GenerateGroupsEndpoint.GetGroupDto>>
                        (groups.Select(g => PlayersMapper.GroupToGroupDto(g)).ToList(), "Groups Fetched Successfully");
                    return Results.Ok(res);
                }, context.TraceIdentifier);
            });
    }
}