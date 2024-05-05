using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class PaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IUserService userService,
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = decimal.Parse(data.Payload);
        var user = await userService.GetAsync(data.User.Id);
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
    }
}