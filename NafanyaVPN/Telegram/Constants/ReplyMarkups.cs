using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Constants;

public static class ReplyMarkups
{
    public static readonly ReplyKeyboardMarkup MainKeyboardMarkup = new ReplyKeyboardMarkup(new []
    {
        new KeyboardButton[]
        {
            MainKeyboardConstants.Donate,
            MainKeyboardConstants.Account,
        },
        new KeyboardButton[]
        {
            MainKeyboardConstants.Instruction,
            MainKeyboardConstants.GetKey
        }
    })
    {
        ResizeKeyboard = true
    };
}