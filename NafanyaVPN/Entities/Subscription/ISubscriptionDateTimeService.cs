namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionDateTimeService
{
    bool IsSubscriptionHasExpired(DateTime subscriptionEndTime);
    DateTime GetNewSubscriptionEndDate();
    TimeSpan GetDelayBetweenChecks();
    DateTime Now();
}