using Microsoft.Extensions.Options;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.DTOs;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ConfirmCustomPaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IPaymentMessageService paymentMessageService,
    IUserService userService,
    IOptions<LaunchOptions> launchOptions,
    ILogger<PaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<PaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        var paymentSum = StringUtils.GetPaymentSumFromTelegramState(user.TelegramState);

        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);
        
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await paymentMessageService.RemoveTelegramPaymentMessageAsync(user.Id);
        await replyService.EditMessageAsync(data.Message,
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
     
        if (launchOptions.Value.AddMoneyOnPaymentCreation)
            await DebugUtils.SendPaymentSuccessNotification(quickpay);
    }
}