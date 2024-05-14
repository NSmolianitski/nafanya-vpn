using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class RenewSubscriptionCommand(
    IUserService userService, 
    IReplyService replyService, 
    ISubscriptionExtendService subscriptionExtendService) 
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        var subscription = user.Subscription;

        if (subscription.HasExpired)
            await subscriptionExtendService.TryRenewForUserAsync(user);
        else
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, "Ваша подписка не истекла.");
    }
}