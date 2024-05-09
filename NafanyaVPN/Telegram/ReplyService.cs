using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram;

public class ReplyService(ITelegramBotClient botClient) : IReplyService
{
    private readonly ReplyKeyboardMarkup _mainKeyboardMarkup = ReplyMarkups.MainKeyboardMarkup;

    public async Task SendTextWithMainKeyboardAsync(long chatId, string text)
    {
        await SendTextWithMarkupAsync(chatId, text, _mainKeyboardMarkup);
    }

    public async Task<Message> SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup)
    {
        return await botClient.SendTextMessageAsync(chatId, text, replyMarkup: markup);
    }

    public async Task EditMessageWithMarkupAsync(Message message, string newText, InlineKeyboardMarkup markup)
    {
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, newText, replyMarkup: markup);
    }

    public async Task EditMessageAsync(Message message, string newText)
    {
        await botClient.EditMessageTextAsync(message.Chat.Id, message.MessageId, newText);
    }

    public async Task SendHelloAsync(long chatId)
    {
        await SendTextWithMainKeyboardAsync(chatId, "Добро пожаловать!");
    }

    public async Task SendChatActionAsync(ChatId chatId, ChatAction chatAction)
    {
        await botClient.SendChatActionAsync(chatId, chatAction);
    }
}