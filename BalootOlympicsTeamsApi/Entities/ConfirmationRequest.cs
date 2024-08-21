namespace BalootOlympicsTeamsApi.Entities;
public class ConfirmationRequest
{
    public Guid Id { get; set; }
    public required string FirstPlayerId { get; set; }
    public required string FirstPlayerOtp { get; set; }
    public required string SecondPlayerId { get; set; }
    public required string SecondPlayerOtp { get; set; }
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ConfirmedAt { get; set; }
}