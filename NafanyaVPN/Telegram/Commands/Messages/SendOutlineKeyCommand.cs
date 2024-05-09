using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class SendOutlineKeyCommand(
    IReplyService replyService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService subscriptionDateTimeService,
    IOutlineKeyService outlineKeyService,
    ISubscriptionExtendService subscriptionExtendService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        if (user.OutlineKey is null)
        {
            await CreateOutlineKeyForUser(user);
            await subscriptionExtendService.TryExtendForUser(user);
        }
        
        if (!subscriptionDateTimeService.IsSubscriptionHasExpired(user.SubscriptionEndDate))
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{user.OutlineKey!.AccessUrl}");
        }
        else
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id,
                $"Ваш ключ временно отключён из-за недостатка средств. На счёте: {user.MoneyInRoubles} рублей");
        }
    }

    private async Task CreateOutlineKeyForUser(User user)
    {
        var keyAccessUrl = outlineService.CreateNewKeyInOutlineManager(user.TelegramUserName, user.TelegramUserId);

        user.OutlineKey = await outlineKeyService.CreateAsync(
            new OutlineKey
            {
                User = user, AccessUrl = keyAccessUrl
            });
    }
}