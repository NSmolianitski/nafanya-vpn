using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.MoneyOperations;

public class MoneyOperation
{
    public int Id { get; set; }
    public string Label { get; set; }
    public DateTime DateTime { get; set; }
    public User User { get; set; }
    public decimal Sum { get; set; }
    public MoneyOperationType Type { get; set; }
}