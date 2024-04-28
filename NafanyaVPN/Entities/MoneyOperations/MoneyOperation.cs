using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.MoneyOperations;

public record MoneyOperation(
    int Id,
    string Label,
    DateTime DateTime,
    User User,
    decimal Sum,
    MoneyOperationType Type)
{
}