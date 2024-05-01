using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Withdraws;

public class Withdraw
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public Subscription.Subscription Subscription { get; set; }
    public decimal Sum { get; set; }
}