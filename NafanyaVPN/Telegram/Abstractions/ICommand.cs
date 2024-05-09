namespace NafanyaVPN.Telegram.Abstractions;

public interface ICommand<T>
{
    Task Execute(T type);
}