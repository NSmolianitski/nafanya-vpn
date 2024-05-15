using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionDateTimeService : ISubscriptionDateTimeService
{
    private readonly TimeSpan _subscriptionLength;
    private readonly TimeSpan _delayBetweenChecks;

    public SubscriptionDateTimeService(IConfiguration configuration, 
        ILogger<SubscriptionDateTimeService> logger)
    {
        var config = configuration.GetRequiredSection(SubscriptionConstants.Subscription);
        _subscriptionLength = TimeSpan.Parse(config[SubscriptionConstants.Length]!);
        _delayBetweenChecks = TimeSpan.Parse(config[SubscriptionConstants.CheckInterval]!);
    }

    public bool HasSubscriptionExpired(Subscription subscription)
    {
        return DateTimeUtils.GetMoscowNowTime() > subscription.EndDateTime;
    }

    public DateTime GetNewSubscriptionEndDateTime()
    {
        return DateTimeUtils.GetMoscowNowTime().Add(_subscriptionLength);
    }
    
    public TimeSpan GetDelayBetweenChecks()
    {
        return _delayBetweenChecks;
    }
}