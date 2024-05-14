using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Withdraws;

public class Withdraw
{
    public Withdraw() {}

    public Withdraw(
        long id, 
        DateTime createdAt, 
        DateTime updatedAt, 
        User user, 
        SubscriptionPlan subscriptionPlan, 
        decimal sum)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        User = user;
        SubscriptionPlan = subscriptionPlan;
        Sum = sum;
    }

    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public decimal Sum { get; set; }
}