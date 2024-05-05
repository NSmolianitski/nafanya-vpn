using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.Abstractions;

public interface IReplyService
{
    Task SendTextWithMainKeyboardAsync(long chatId, string text);
    Task SendTextWithMarkupAsync(long chatId, string text, IReplyMarkup markup);
    Task SendHelloAsync(long chatId);
}