
// using FluentValidation;

// namespace BalootOlympicsTeamsApi.Modules.Groups;

// public sealed class GenerateGroupsService(GroupRepo _groupRepo, TeamRepo _teamRepo)
// {
//     private static List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> GenerateGroupTeamsCount(int teamsCount, int groupsCount, List<DateTime> dates)
//     {
//         int baseTeamsCountPerGroup = teamsCount / groupsCount;
//         int teamsOverCount = teamsCount % groupsCount;
//         int dateIndex = 0;
//         List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> teamsGroupCount = [];
//         for (int i = 0; i < groupsCount; i++)
//         {
//             int group_count = baseTeamsCountPerGroup;
//             if (teamsOverCount > 0)
//             {
//                 group_count++; teamsOverCount--;
//             }
//             // teamsGroupCount.Add((group_count, dates[dateIndex],));
//             dateIndex = (dateIndex + 1) % dates.Count;
//         }
//         return teamsGroupCount;
//     }
//     // Task<Result<List<Team>>>
//     public async void ExecuteAsync(GenerateGroupsEndpoint.GenerateGroupsRequestDto dto)
//     {
//         (await _teamRepo.GetAllApprovedAsync())
//            //Delete Any Group found
//            .OnSuccessAsync((teams) =>
//            {
//                List<(int TeamsCount, DateTime PlayAtDate, int indexOfGroupInThisDate)> teamsGroupCount = GenerateGroupTeamsCount(teams.Count, dto.GroupsCount, dto.ChampionshipDays);
//                List<Group> ChampGroups = [];
//                // foreach ((int teamsCountPerGroup, DateTime GroupPlayAtDate, int GroupIndexInThisDate) in teamsGroupCount)
//                // {
//                //     // create group 
//                //     // add teams to it and remove it from teams array 
//                //     // 
//                //     var g = new Group()
//                //     {
//                //         StartPlayAt = DateTimeOffset.UtcNow,
//                //         CheckInAt = DateTimeOffset.UtcNow,
//                //         CompetingTeams = [],
//                //         Name = ""
//                //     };
//                //     ChampGroups.Add(g);
//                // }
//                return Result.Ok(teams);
//            });
//     }
// }
// public sealed class GenerateGroupsRequestDtoValidator : AbstractValidator<GenerateGroupsEndpoint.GenerateGroupsRequestDto>
// {
//     public GenerateGroupsRequestDtoValidator()
//     {
//         RuleFor(x => x.GroupsCount).NotEmpty().GreaterThan(0);
//         RuleFor(x => x.QualifiedTeamsCountPerGroup).GreaterThan(0)
//         .Must((num) =>
//         {
//             if (num <= 0)
//                 return false;
//             return (num & (num - 1)) == 0;
//         }).WithMessage("QualifiedTeamsCountPerGroup Must be number of power of 2.");
//         RuleFor(x => x.ChampionshipDays).Must((list) => list.Count > 0);
//         RuleForEach(x => x.ChampionshipDays).GreaterThanOrEqualTo(DateTime.UtcNow.Date);
//     }
// }
// public sealed class GenerateGroupsEndpoint : CarterModule
// {
//     public record GenerateGroupsRequestDto(int GroupsCount, int QualifiedTeamsCountPerGroup, List<DateTime> ChampionshipDays);
//     public override void AddRoutes(IEndpointRouteBuilder app)
//     {
//         app.MapGet("/groups/generate",
//             async Task<IResult> (GenerateGroupsRequestDto dto, HttpContext context, [FromServices] GenerateGroupsService service) =>
//             {
//                 // await service.ExecuteAsync(dto);
//                 return Results.Ok();
//             });

//     }

// }