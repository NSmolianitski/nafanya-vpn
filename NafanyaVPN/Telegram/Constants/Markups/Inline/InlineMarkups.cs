using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Constants;

public static partial class InlineMarkups
{
    private static readonly string PaymentSumPrefix = $"{CallbackConstants.ConfirmPaymentSum}" +
                                                      $"{CallbackConstants.SplitSymbol}";
    
    public static readonly InlineKeyboardMarkup PaymentSum = new InlineKeyboardMarkup(new []
    {
        new InlineKeyboardButton[] 
        {
            InlineKeyboardButton.WithCallbackData("100", $"{PaymentSumPrefix}100"),
            InlineKeyboardButton.WithCallbackData("200", $"{PaymentSumPrefix}200"),
            InlineKeyboardButton.WithCallbackData("300", $"{PaymentSumPrefix}300")
        },
        new InlineKeyboardButton[] 
        {
            InlineKeyboardButton.WithCallbackData("500", $"{PaymentSumPrefix}500"),
            InlineKeyboardButton.WithCallbackData("1000", $"{PaymentSumPrefix}1000"),
            InlineKeyboardButton.WithCallbackData("2000", $"{PaymentSumPrefix}2000"),
        },
        new InlineKeyboardButton[] 
        {
            InlineKeyboardButton.WithCallbackData("Другая", CallbackConstants.CustomPaymentSum)
        }
    });
}