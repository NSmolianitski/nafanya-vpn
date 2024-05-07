using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Subscription;

public class SubscriptionDateTimeService(ILogger<SubscriptionDateTimeService> logger) : ISubscriptionDateTimeService
{
    private readonly ILogger<SubscriptionDateTimeService> _logger = logger;

    public bool IsSubscriptionActive(DateTime subscriptionEndTime)
    {
        return DateTimeUtils.GetMoscowNowTime().Date < subscriptionEndTime.Date;
    }

    public DateTime GetNewSubscriptionEndDate()
    {
        return DateTimeUtils.GetMoscowNowTime().AddDays(1).Date;
    }
    
    public TimeSpan GetDelayForNextSubscriptionUpdate()
    {
        var moscowNowTime = DateTimeUtils.GetMoscowNowTime();
        var nextMidnight = moscowNowTime.AddDays(1).Date;
        var delay = nextMidnight - moscowNowTime;
        
        return delay;
    }

    public DateTime Now()
    {
        return DateTimeUtils.GetMoscowNowTime();
    }
}