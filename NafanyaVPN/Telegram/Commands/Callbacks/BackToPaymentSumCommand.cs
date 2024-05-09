using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Commands.Messages;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class BackToPaymentSumCommand(IUserService userService, IReplyService replyService, ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = InlineMarkups.PaymentSum;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);
        
        await replyService.EditMessageWithMarkupAsync(data.Message, "Выберите сумму:", _replyMarkup);
    }
}