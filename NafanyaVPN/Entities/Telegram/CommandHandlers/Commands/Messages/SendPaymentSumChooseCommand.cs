using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendPaymentSumChooseCommand(
    IReplyService replyService, 
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<MessageDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = Markups.PaymentSum;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(MessageDto data)
    {
        await replyService.SendTextWithMarkupAsync(data.Message.Chat.Id, "Выберите сумму:", _replyMarkup);
    }
}