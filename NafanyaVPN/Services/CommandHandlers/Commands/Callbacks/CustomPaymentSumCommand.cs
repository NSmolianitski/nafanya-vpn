using NafanyaVPN.Constants;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Callbacks;

public class CustomPaymentSumCommand : ICommand<CallbackQueryDto>
{
    private readonly IUserService _userService;
    private readonly IReplyService _replyService;
    private readonly ILogger<CustomPaymentSumCommand> _logger;

    public CustomPaymentSumCommand(IUserService userService, IReplyService replyService, 
        ILogger<CustomPaymentSumCommand> logger)
    {
        _userService = userService;
        _replyService = replyService;
        _logger = logger;
    }

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await _userService.GetAsync(data.User.Id);
        user.TelegramState = TelegramUserStateConstants.CustomPaymentSum;
        await _userService.UpdateAsync(user);
        
        await _replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, "Введите сумму вручную (минимум 2 рубля)");
    }
}