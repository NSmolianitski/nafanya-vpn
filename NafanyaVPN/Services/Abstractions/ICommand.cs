namespace NafanyaVPN.Services.Abstractions;

public interface ICommand<T>
{
    Task Execute(T type);
}