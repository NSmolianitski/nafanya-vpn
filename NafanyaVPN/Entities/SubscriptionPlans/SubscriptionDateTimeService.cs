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
        _subscriptionLength = TimeSpan.Parse(config[SubscriptionConstants.SubscriptionLength]!);
        _delayBetweenChecks = TimeSpan
            .Parse(config[SubscriptionConstants.SubscriptionCheckInterval]!);
    }

    public bool IsSubscriptionHasExpired(DateTime subscriptionEndTime)
    {
        return DateTimeUtils.GetMoscowNowTime() > subscriptionEndTime;
    }

    public DateTime GetNewSubscriptionEndDate()
    {
        return DateTimeUtils.GetMoscowNowTime().Add(_subscriptionLength);
    }
    
    public TimeSpan GetDelayBetweenChecks()
    {
        return _delayBetweenChecks;
    }

    public DateTime Now()
    {
        return DateTimeUtils.GetMoscowNowTime();
    }
}