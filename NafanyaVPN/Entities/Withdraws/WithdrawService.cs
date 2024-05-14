using NafanyaVPN.Entities.Subscriptions;

namespace NafanyaVPN.Entities.Withdraws;

public class WithdrawService(IWithdrawRepository withdrawRepository) : IWithdrawService
{
    public Withdraw CreateWithoutSaving(Subscription subscription)
    {
        var subscriptionPlan = subscription.SubscriptionPlan;
        
        var withdraw = new WithdrawBuilder()
            .WithNowCreatedAtUpdatedAt()
            .WithSum(subscriptionPlan.CostInRoubles)
            .WithUser(subscription.User)
            .WithSubscriptionPlan(subscriptionPlan)
            .Build();
        return withdrawRepository.AddWithoutSaving(withdraw);
    }
}