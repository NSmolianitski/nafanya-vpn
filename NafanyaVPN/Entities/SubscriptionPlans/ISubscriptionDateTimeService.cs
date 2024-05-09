namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionDateTimeService
{
    bool IsSubscriptionHasExpired(DateTime subscriptionEndTime);
    DateTime GetNewSubscriptionEndDate();
    TimeSpan GetDelayBetweenChecks();
    DateTime Now();
}