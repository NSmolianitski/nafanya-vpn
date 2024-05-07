namespace NafanyaVPN.Utils;

public static class DateTimeUtils
{
    public static DateTime GetMoscowNowTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
    }
}