namespace NafanyaVPN.Services.Abstractions;

public interface ISubscriptionDateTimeService
{
    bool IsSubscriptionActive(DateTime subscriptionEndTime);
    DateTime GetNewSubscriptionEndDate();
    TimeSpan GetDelayForNextSubscriptionUpdate();
    DateTime Now();
}