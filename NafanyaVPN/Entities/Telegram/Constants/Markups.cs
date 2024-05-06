using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.Constants;

public static class Markups
{
    private static readonly string PaymentSumPrefix = $"{CallbackConstants.PaymentSum}{CallbackConstants.SplitSymbol}";
    
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
            InlineKeyboardButton.WithCallbackData("Другая", CallbackConstants.CustomPaymentSum)
        }
    });

    private static readonly string CustomPaymentPrefix = $"{CallbackConstants.CustomPaymentSum}{CallbackConstants.SplitSymbol}";

    public static readonly InlineKeyboardMarkup CustomPaymentSum = new InlineKeyboardMarkup(new[]
    {
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("+1", $"{CustomPaymentPrefix}+1"),
            InlineKeyboardButton.WithCallbackData("+10", $"{CustomPaymentPrefix}+10"),
            InlineKeyboardButton.WithCallbackData("+50", $"{CustomPaymentPrefix}+50"),
            InlineKeyboardButton.WithCallbackData("+100", $"{CustomPaymentPrefix}+100"),
            InlineKeyboardButton.WithCallbackData("+500", $"{CustomPaymentPrefix}+500"),
            InlineKeyboardButton.WithCallbackData("+1000", $"{CustomPaymentPrefix}+1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("-1", $"{CustomPaymentPrefix}-1"),
            InlineKeyboardButton.WithCallbackData("-10", $"{CustomPaymentPrefix}-10"),
            InlineKeyboardButton.WithCallbackData("-50", $"{CustomPaymentPrefix}-50"),
            InlineKeyboardButton.WithCallbackData("-100", $"{CustomPaymentPrefix}-100"),
            InlineKeyboardButton.WithCallbackData("-500", $"{CustomPaymentPrefix}-500"),
            InlineKeyboardButton.WithCallbackData("-1000", $"{CustomPaymentPrefix}-1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("Подтвердить", CallbackConstants.ConfirmPaymentSum),
            InlineKeyboardButton.WithCallbackData("Назад", CallbackConstants.BackToPaymentSum)
        }
    });
}