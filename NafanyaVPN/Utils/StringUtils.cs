using NafanyaVPN.Entities.Telegram.Constants;

namespace NafanyaVPN.Utils;

public static class StringUtils
{
    public static string GetUniqueLabel()
    {
        return Guid.NewGuid().ToString();
    }

    public static bool HasSplitSymbol(string str)
    {
        return str.Contains(CallbackConstants.SplitSymbol); 
    }
    
    public static string[] SplitBySymbol(string str)
    {
        return str.Split(CallbackConstants.SplitSymbol);
    }
    
    public static decimal GetPaymentSumFromTelegramState(string telegramState)
    {
        return decimal.Parse(telegramState.Split(CallbackConstants.SplitSymbol)[1]);
    }
    
    public static decimal ParseSum(string str)
    {
        return decimal.Parse(str);
    }
}