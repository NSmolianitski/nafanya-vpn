using NafanyaVPN.Entities.Subscriptions;
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
        }
    })
    {
        ResizeKeyboard = true
    };
    
    public static readonly ReplyKeyboardMarkup MainKeyboardWithRenewMarkup = new ReplyKeyboardMarkup(new []
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

    public static ReplyKeyboardMarkup CreateMainMarkup(Subscription subscription)
    {
        return subscription.RenewalDisabled 
            ? MainKeyboardWithRenewMarkup 
            : MainKeyboardMarkup;
    }
}