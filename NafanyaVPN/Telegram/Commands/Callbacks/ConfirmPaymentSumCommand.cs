using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.DTOs;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ConfirmPaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IUserService userService,
    IPaymentMessageService paymentMessageService,
    ILogger<PaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<PaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = StringUtils.ParseSum(data.Payload);
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await paymentMessageService.RemoveTelegramPaymentMessageAsync(user.Id);
        await replyService.EditMessageAsync(data.Message,
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
    }
}