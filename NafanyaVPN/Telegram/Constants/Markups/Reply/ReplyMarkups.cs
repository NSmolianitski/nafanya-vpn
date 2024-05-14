using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Constants;

public static class ReplyMarkups
{
    public static readonly ReplyKeyboardMarkup MainKeyboardMarkup = new ReplyKeyboardMarkup(new []
    {
        new KeyboardButton[]
        {
            MainKeyboardConstants.Account,
        },
        new KeyboardButton[]
        {
            MainKeyboardConstants.Donate,
            MainKeyboardConstants.GetKey
        },
        new KeyboardButton[]
        {
            MainKeyboardConstants.Instruction,
            MainKeyboardConstants.Settings,
        },
        new KeyboardButton[]
        {
            MainKeyboardConstants.RenewSubscription,
        }
    })
    {
        ResizeKeyboard = true
    };
}