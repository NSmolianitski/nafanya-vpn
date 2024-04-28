using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Services;

public class SubscriptionDateTimeService(ILogger<SubscriptionDateTimeService> logger) : ISubscriptionDateTimeService
{
    private readonly ILogger<SubscriptionDateTimeService> _logger = logger;

    public bool IsSubscriptionActive(DateTime subscriptionEndTime)
    {
        return DateTimeUtils.GetMoscowTime().Date < subscriptionEndTime.Date;
    }

    public DateTime GetNewSubscriptionEndDate()
    {
        return DateTimeUtils.GetMoscowTime().AddDays(1).Date;
    }
    
    public TimeSpan GetDelayForNextSubscriptionUpdate()
    {
        var moscowNowTime = DateTimeUtils.GetMoscowTime();
        var nextMidnight = moscowNowTime.AddDays(1).Date;
        var delay = nextMidnight - moscowNowTime;
        
        return delay;
    }

    public DateTime Now()
    {
        return DateTimeUtils.GetMoscowTime();
    }
}