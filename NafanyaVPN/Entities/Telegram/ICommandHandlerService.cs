namespace NafanyaVPN.Entities.Telegram;

public interface ICommandHandlerService<T>
{
    Task HandleCommand(T data);
}