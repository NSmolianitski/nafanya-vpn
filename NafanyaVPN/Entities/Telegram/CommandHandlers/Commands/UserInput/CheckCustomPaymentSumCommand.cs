using NafanyaVPN.Entities.PaymentNotifications;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.UserInput;

public class CheckCustomPaymentSumCommand(
    IReplyService replyService,
    IUserService userService,
    IPaymentService paymentService)
    : ICommand<UserInputDto>
{
    public async Task Execute(UserInputDto data)
    {
        if (!TryParseCustomSum(data.Payload, out var paymentSum, out var errorMessage))
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{errorMessage}: {data.Payload}");
            return;
        }

        var user = await userService.GetAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);

        var paymentLabel = StringUtils.GetUniqueLabel();
        var quickpay = paymentService.GetPaymentForm(paymentSum, paymentLabel);
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
        
        // TODO: проверить совершение оплаты
        // var paymentResult = await paymentService.ListenForPayment(paymentLabel);paymentService
        // Console.WriteLine(paymentResult);
        
        // await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentResult}");
    }

    private bool TryParseCustomSum(string sumInput, out decimal paymentSum, out string errorMessage)
    {
        paymentSum = 0;
        
        // TODO: check sum conversion
        var isDecimal = decimal.TryParse(sumInput, out var parsedSum);
        if (!isDecimal)
        {
            errorMessage = "Введённая сумма не является числом, либо число слишком большое";
            return false;
        }

        if (parsedSum < 2)
        {
            errorMessage = "Минимальное значение для суммы - 2 рубля";
            return false;
        }

        paymentSum = parsedSum;
        errorMessage = string.Empty;
        return true;
    }
}