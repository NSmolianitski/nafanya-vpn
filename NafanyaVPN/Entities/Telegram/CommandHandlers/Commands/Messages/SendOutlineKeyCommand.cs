using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;
using Telegram.Bot.Types;
using User = NafanyaVPN.Entities.Users.User;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendOutlineKeyCommand(
    IReplyService replyService,
    IUserService userService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService subscriptionDateTimeService,
    IOutlineKeysService outlineKeysService,
    ISubscriptionExtendService subscriptionExtendService)
    : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        var telegramUser = message.From;
        var user = await userService.GetAsync(telegramUser!.Id);
        
        if (user.OutlineKey is null)
        {
            await CreateOutlineKeyForUser(user);
            await subscriptionExtendService.TryExtendForUser(user);
        }
        
        if (subscriptionDateTimeService.IsSubscriptionActive(user.SubscriptionEndDate))
        {
            await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{user.OutlineKey!.AccessUrl}");
        }
        else
        {
            await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id,
                $"Ваш ключ временно деактивирован. Пополните счёт. Остаток: {user.MoneyInRoubles} рублей");
        }
    }

    private async Task CreateOutlineKeyForUser(User user)
    {
        var keyAccessUrl = outlineService.GetNewKey(user.TelegramUserName, user.TelegramUserId);

        user.OutlineKey = await outlineKeysService.CreateAsync(
            new OutlineKey
            {
                User = user, AccessUrl = keyAccessUrl
            });
    }
}