using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.Constants;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendPaymentSumChooseCommand(
    IReplyService replyService, 
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<Message>
{
    private readonly InlineKeyboardMarkup _replyMarkup = Markups.PaymentSum;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(Message message)
    {
        await replyService.SendTextWithMarkupAsync(message.Chat.Id, "Выберите сумму:", _replyMarkup);
    }
}