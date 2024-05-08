using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class ConfirmPaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IUserService userService,
    IPaymentMessageService paymentMessageService,
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var paymentSum = StringUtils.ParseSum(data.Payload);
        var user = await userService.GetAsync(data.User.Id);
        
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await paymentMessageService.RemoveTelegramPaymentMessageAsync(user.Id);
        await replyService.EditMessageAsync(data.Message,
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
    }
}