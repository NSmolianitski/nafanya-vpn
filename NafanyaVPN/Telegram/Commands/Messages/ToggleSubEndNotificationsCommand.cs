using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ToggleSubEndNotificationsCommand(
    ISubscriptionService subscriptionService, 
    IReplyService replyService) 
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var subscription = data.User.Subscription;
        subscription.EndNotificationsDisabled = !subscription.EndNotificationsDisabled;
        
        await subscriptionService.UpdateAsync(subscription);
        var replyMarkup = ReplyMarkups.CreateSettingsMarkup(subscription.RenewalDisabled,
            subscription.RenewalNotificationsDisabled, subscription.EndNotificationsDisabled);
        
        await replyService.SendTextWithMarkupAsync(data.Message.Chat.Id, MainKeyboardConstants.Settings, replyMarkup);
    }
}