using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram;

public class ReplyService : IReplyService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ReplyKeyboardMarkup _mainKeyboardMarkup;
    
    public ReplyService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
        
        var keyboardButtons = new []
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
        };
        
        _mainKeyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons)
        {
            ResizeKeyboard = true
        };
    }
    
    public async Task SendTextWithMainKeyboardAsync(long chatId, string text)
    {
        await SendTextWithMarkupAsync(chatId, text, _mainKeyboardMarkup);
    }

    public async Task SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup)
    {
        await _botClient.SendTextMessageAsync(chatId, text, replyMarkup: markup);
    }

    public async Task SendHelloAsync(long chatId)
    {
        await SendTextWithMainKeyboardAsync(chatId, "Добро пожаловать!");
    }
}