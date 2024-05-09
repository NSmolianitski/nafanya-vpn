namespace NafanyaVPN.Telegram.Abstractions;

public interface ICommandHandlerService<T>
{
    Task HandleCommand(T data);
}