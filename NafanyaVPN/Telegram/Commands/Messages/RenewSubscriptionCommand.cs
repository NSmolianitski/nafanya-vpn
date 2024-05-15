using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class RenewSubscriptionCommand(
    IUserService userService, 
    IReplyService replyService, 
    ISubscriptionRenewService subscriptionRenewService) 
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.TelegramUserId);
        var subscription = user.Subscription;

        if (subscription.HasExpired)
        {
            await subscriptionRenewService.RenewIfEnoughMoneyAsync(user);
        }
        else
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, subscription,
                "Ваша подписка не истекла.");
        }
    }
}