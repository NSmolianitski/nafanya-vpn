using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Callbacks;

public class CustomPaymentSumCommand(
    IUserService userService,
    IReplyService replyService,
    ILogger<CustomPaymentSumCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly ILogger<CustomPaymentSumCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetAsync(data.User.Id);
        user.TelegramState = TelegramUserStateConstants.CustomPaymentSum;
        await userService.UpdateAsync(user);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, "Введите сумму вручную (минимум 2 рубля)");
    }
}