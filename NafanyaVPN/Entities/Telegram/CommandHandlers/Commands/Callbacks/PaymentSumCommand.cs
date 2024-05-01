using NafanyaVPN.Entities.PaymentNotifications;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class PaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = decimal.Parse(data.Payload);
        var paymentLabel = StringUtils.GetUniqueLabel();
        var quickpay = paymentService.GetPaymentForm(paymentSum, paymentLabel);
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
        
        // TODO: проверить совершение оплаты
        // var paymentResult = await paymentService.ListenForPayment(paymentLabel);
        
        // await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentResult}");
    }
}