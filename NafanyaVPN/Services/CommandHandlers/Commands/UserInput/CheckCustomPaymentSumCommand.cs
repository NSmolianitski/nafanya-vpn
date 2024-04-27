using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Services.CommandHandlers.Commands.UserInput;

public class CheckCustomPaymentSumCommand : ICommand<UserInputDto>
{
    private readonly IUserService _userService;
    private readonly IReplyService _replyService;
    private readonly IPaymentService _paymentService;

    public CheckCustomPaymentSumCommand(
        IReplyService replyService, 
        IUserService userService, 
        IPaymentService paymentService)
    {
        _replyService = replyService;
        _userService = userService;
        _paymentService = paymentService;
    }

    public async Task Execute(UserInputDto data)
    {
        if (!TryParseCustomSum(data.Payload, out var paymentSum, out var errorMessage))
        {
            await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{errorMessage}: {data.Payload}");
            return;
        }

        var user = await _userService.GetAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await _userService.UpdateAsync(user);

        var paymentLabel = StringUtils.GetUniqueLabel();
        var quickpay = _paymentService.GetPaymentForm(paymentSum, paymentLabel);
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Ваша ссылка для оплаты: {quickpay.LinkPayment}");
        
        var paymentResult = await _paymentService.ListenForPayment(paymentLabel);
        Console.WriteLine(paymentResult);
        
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentResult}");
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