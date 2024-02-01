namespace NafanyaVPN.Models;

public class MoneyOperation
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public virtual User User { get; set; }
    public decimal Sum { get; set; }
    public virtual MoneyOperationType Type { get; set; }
}