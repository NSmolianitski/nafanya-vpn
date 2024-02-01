namespace NafanyaVPN.Utils;

public static class DateTimeUtils
{
    public static DateTime GetMoscowTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
    }
}