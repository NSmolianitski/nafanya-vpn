using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ToggleSubEndNotificationsCommand(
    IUserService userService, 
    ISubscriptionService subscriptionService, 
    IReplyService replyService) 
    : ICommand<CallbackQueryDto>
{
    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        var subscription = user.Subscription;
        subscription.EndNotificationsDisabled = !subscription.EndNotificationsDisabled;
        
        await subscriptionService.UpdateAsync(subscription);
        var replyMarkup = InlineMarkups.CreateSettingsMarkup(subscription.RenewalDisabled,
            subscription.RenewalNotificationsDisabled, subscription.EndNotificationsDisabled);
        
        await replyService.EditMessageWithMarkupAsync(data.Message, MainKeyboardConstants.Settings, replyMarkup);
    }
}