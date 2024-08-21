namespace BalootOlympicsTeamsApi.Common;
public interface IAsyncCommandService<TCommand, TResult>
{
    Task<TResult> ExecuteAsync(TCommand command);
}

