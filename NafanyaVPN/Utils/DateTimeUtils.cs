using System.Globalization;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Telegram.Constants;

namespace NafanyaVPN.Utils;

public static class DateTimeUtils
{
    public static DateTime GetMoscowNowTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
    }

    public static TimeSpan GetTimeUntilDateTime(DateTime dateTime)
    {
        return GetMoscowNowTime().Subtract(dateTime).Duration();
    }
    
    public static string GetSubEndString(Subscription subscription)
    {
        return subscription.EndDateTime.ToString(TelegramConstants.DateTimeFormat, CultureInfo.InvariantCulture);
    }
}