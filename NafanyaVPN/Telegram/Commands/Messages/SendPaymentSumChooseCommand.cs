using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class SendPaymentSumChooseCommand(
    IReplyService replyService,
    IPaymentMessageService paymentMessageService,
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<MessageDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = InlineMarkups.PaymentSum;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(MessageDto data)
    {
        var message = await replyService.SendTextWithMarkupAsync(data.Message.Chat.Id, 
            "Выберите сумму:", _replyMarkup);
        await paymentMessageService.CreateAsync(message, data.User);
    }
}