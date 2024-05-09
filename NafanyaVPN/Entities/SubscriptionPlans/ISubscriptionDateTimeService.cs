using NafanyaVPN.Entities.Subscriptions;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionDateTimeService
{
    bool HasSubscriptionExpired(Subscription subscription);
    DateTime GetNewSubscriptionEndDateTime();
    TimeSpan GetDelayBetweenChecks();
}