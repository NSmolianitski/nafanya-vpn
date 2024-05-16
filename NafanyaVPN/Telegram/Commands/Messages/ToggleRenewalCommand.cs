using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ToggleRenewalCommand(
    ISubscriptionService subscriptionService, 
    ISubscriptionRenewService subscriptionRenewService, 
    IReplyService replyService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        var subscription = user.Subscription;
        subscription.RenewalDisabled = !subscription.RenewalDisabled;
        
        await subscriptionService.UpdateAsync(subscription);
        var replyMarkup = ReplyMarkups.CreateSettingsMarkup(subscription.RenewalDisabled,
            subscription.RenewalNotificationsDisabled, subscription.EndNotificationsDisabled);
        
        await replyService.SendTextWithMarkupAsync(data.Message.Chat.Id, MainKeyboardConstants.Settings, replyMarkup);

        // Обновление подписки в случае, если пользователь включает автопродление и подписка истекла
        if (subscription is { HasExpired: true, RenewalDisabled: false })
            await subscriptionRenewService.RenewIfEnoughMoneyAsync(user);
    }
}