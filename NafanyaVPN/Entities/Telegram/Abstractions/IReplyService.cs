using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.Abstractions;

public interface IReplyService
{
    Task SendTextWithMainKeyboardAsync(long chatId, string text);
    Task SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup);
    Task EditMessageWithMarkupAsync(Message message, string newText, InlineKeyboardMarkup markup);
    Task EditMessageAsync(Message message, string newText);
    Task SendHelloAsync(long chatId);
}