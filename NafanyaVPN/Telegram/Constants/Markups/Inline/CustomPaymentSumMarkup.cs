using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Constants;

public static partial class InlineMarkups
{
    private static readonly string CustomPaymentPrefix = $"{CallbackConstants.CustomPaymentSum}" +
                                                         $"{CallbackConstants.SplitSymbol}";

    public static readonly InlineKeyboardMarkup CustomPaymentSum = new InlineKeyboardMarkup(new[]
    {
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("+10", $"{CustomPaymentPrefix}+10"),
            InlineKeyboardButton.WithCallbackData("+50", $"{CustomPaymentPrefix}+50"),
            InlineKeyboardButton.WithCallbackData("+100", $"{CustomPaymentPrefix}+100"),
            InlineKeyboardButton.WithCallbackData("+500", $"{CustomPaymentPrefix}+500"),
            InlineKeyboardButton.WithCallbackData("+1000", $"{CustomPaymentPrefix}+1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("-10", $"{CustomPaymentPrefix}-10"),
            InlineKeyboardButton.WithCallbackData("-50", $"{CustomPaymentPrefix}-50"),
            InlineKeyboardButton.WithCallbackData("-100", $"{CustomPaymentPrefix}-100"),
            InlineKeyboardButton.WithCallbackData("-500", $"{CustomPaymentPrefix}-500"),
            InlineKeyboardButton.WithCallbackData("-1000", $"{CustomPaymentPrefix}-1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", CallbackConstants.BackToPaymentSum),
            InlineKeyboardButton.WithCallbackData("Подтвердить", CallbackConstants.ConfirmCustomPaymentSum)
        }
    });

    public static readonly InlineKeyboardMarkup CustomPaymentSumLowBorder = new InlineKeyboardMarkup(new[]
    {
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("+10", $"{CustomPaymentPrefix}+10"),
            InlineKeyboardButton.WithCallbackData("+50", $"{CustomPaymentPrefix}+50"),
            InlineKeyboardButton.WithCallbackData("+100", $"{CustomPaymentPrefix}+100"),
            InlineKeyboardButton.WithCallbackData("+500", $"{CustomPaymentPrefix}+500"),
            InlineKeyboardButton.WithCallbackData("+1000", $"{CustomPaymentPrefix}+1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", CallbackConstants.BackToPaymentSum)
        }
    });

    public static readonly InlineKeyboardMarkup CustomPaymentSumHighBorder = new InlineKeyboardMarkup(new[]
    {
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("-10", $"{CustomPaymentPrefix}-10"),
            InlineKeyboardButton.WithCallbackData("-50", $"{CustomPaymentPrefix}-50"),
            InlineKeyboardButton.WithCallbackData("-100", $"{CustomPaymentPrefix}-100"),
            InlineKeyboardButton.WithCallbackData("-500", $"{CustomPaymentPrefix}-500"),
            InlineKeyboardButton.WithCallbackData("-1000", $"{CustomPaymentPrefix}-1000")
        },
        new InlineKeyboardButton[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", CallbackConstants.BackToPaymentSum),
            InlineKeyboardButton.WithCallbackData("Подтвердить", CallbackConstants.ConfirmCustomPaymentSum)
        }
    });
}