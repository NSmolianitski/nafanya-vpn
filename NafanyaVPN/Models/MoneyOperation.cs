namespace NafanyaVPN.Models;

public record MoneyOperation(
    int Id,
    string Label,
    DateTime DateTime,
    User User,
    decimal Sum,
    MoneyOperationType Type)
{
}