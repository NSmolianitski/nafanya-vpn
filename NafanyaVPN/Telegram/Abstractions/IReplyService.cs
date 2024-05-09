using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Abstractions;

public interface IReplyService
{
    Task SendTextWithMainKeyboardAsync(long chatId, string text);
    Task<Message> SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup);
    Task EditMessageWithMarkupAsync(Message message, string newText, InlineKeyboardMarkup markup);
    Task EditMessageAsync(Message message, string newText);
    Task SendHelloAsync(long chatId);
    Task SendChatActionAsync(ChatId chatId, ChatAction chatAction);
}