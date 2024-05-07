using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class ConfirmCustomPaymentSumCommand(
    IReplyService replyService,
    IPaymentService paymentService,
    IPaymentMessageService paymentMessageService,
    IUserService userService,
    ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetAsync(data.User.Id);
        var paymentSum = StringUtils.GetPaymentSumFromTelegramState(user.TelegramState);

        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);
        
        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        
        await paymentMessageService.ClearPaymentMessageAsync(user.Id);
        await replyService.EditMessageAsync(data.Message,
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
    }
}