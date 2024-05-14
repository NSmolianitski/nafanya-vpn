using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Withdraws;

public interface IWithdrawService
{
    Withdraw CreateWithoutSaving(Subscription subscription);
}

