namespace NafanyaVPN.Utils;

public static class StringUtils
{
    public static string GetUniqueLabel()
    {
        return Guid.NewGuid().ToString();
    }
}