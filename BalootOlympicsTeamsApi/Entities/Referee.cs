namespace BalootOlympicsTeamsApi.Entities;
public class Referee
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
}