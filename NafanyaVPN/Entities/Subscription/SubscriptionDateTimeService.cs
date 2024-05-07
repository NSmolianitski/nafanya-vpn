using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Subscription;

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

    public bool IsSubscriptionActive(DateTime subscriptionEndTime)
    {
        return DateTimeUtils.GetMoscowNowTime() < subscriptionEndTime;
    }

    public DateTime GetNewSubscriptionEndDate()
    {
        return DateTimeUtils.GetMoscowNowTime().Add(_subscriptionLength);
    }
    
    public TimeSpan GetDelayUntilNextUpdate()
    {
        var moscowNowTime = DateTimeUtils.GetMoscowNowTime();
        // var nextMidnight = moscowNowTime.AddDays(1).Date;
        // var delay = nextMidnight - moscowNowTime;
        
        // return delay;
        return _delayBetweenChecks;
    }

    public DateTime Now()
    {
        return DateTimeUtils.GetMoscowNowTime();
    }
}