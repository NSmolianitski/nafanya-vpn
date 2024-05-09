using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.UserInput;

// Ручной выбор суммы (через ввод текстом). В данный момент не используется
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

        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);

        var quickpay = await paymentService.CreatePaymentFormAsync(paymentSum, user);
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Совершите оплату по ссылке: {quickpay.LinkPayment}");
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