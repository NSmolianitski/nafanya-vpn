using NafanyaVPN.Services.Abstractions;

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
        // TODO: check sum conversion
        if (!decimal.TryParse(data.Payload, out var paymentSum))
        {
            await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"Введённая сумма некорректна: {data.Payload}");
            return;
        }

        var user = await _userService.GetAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await _userService.UpdateAsync(user);

        await _paymentService.SendPaymentForm(paymentSum, data.User.Id);
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{paymentSum}");
    }
}