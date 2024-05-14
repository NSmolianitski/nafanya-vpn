using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class SettingsCommand(
    IReplyService replyService, 
    ILogger<PaymentSumChooseCommand> logger)
    : ICommand<MessageDto>
{
    private readonly ILogger<PaymentSumChooseCommand> _logger = logger;

    public async Task Execute(MessageDto data)
    {
        var subscription = data.User.Subscription;
        var replyMarkup = InlineMarkups.CreateSettingsMarkup(
            subscription.RenewalDisabled, 
            subscription.RenewalNotificationsDisabled, 
            subscription.EndNotificationsDisabled);
        
        await replyService.SendTextWithMarkupAsync(data.Message.Chat.Id, MainKeyboardConstants.Settings, replyMarkup);
    }
}