using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Withdraws;

public class Withdraw
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public decimal Sum { get; set; }
}