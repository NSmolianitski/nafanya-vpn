namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionDateTimeService
{
    bool IsSubscriptionActive(DateTime subscriptionEndTime);
    DateTime GetNewSubscriptionEndDate();
    TimeSpan GetDelayUntilNextUpdate();
    DateTime Now();
}