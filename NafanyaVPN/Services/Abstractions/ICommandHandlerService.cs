namespace NafanyaVPN.Services.Abstractions;

public interface ICommandHandlerService<T>
{
    Task HandleCommand(T data);
}