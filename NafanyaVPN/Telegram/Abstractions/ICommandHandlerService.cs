namespace NafanyaVPN.Entities.Telegram.Abstractions;

public interface ICommandHandlerService<T>
{
    Task HandleCommand(T data);
}