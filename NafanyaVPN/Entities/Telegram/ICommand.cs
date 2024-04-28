namespace NafanyaVPN.Entities.Telegram;

public interface ICommand<T>
{
    Task Execute(T type);
}