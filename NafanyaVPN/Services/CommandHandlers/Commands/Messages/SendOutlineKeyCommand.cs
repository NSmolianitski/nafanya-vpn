using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;
using User = NafanyaVPN.Models.User;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendOutlineKeyCommand : ICommand<Telegram.Bot.Types.Message>
{
    private readonly IReplyService _replyService;
    private readonly IUserService _userService;
    private readonly IOutlineService _outlineService;
    private readonly IOutlineKeysService _outlineKeysService;
    private readonly ISubscriptionDateTimeService _subscriptionDateTimeService;
    private readonly ISubscriptionExtendService _subscriptionExtendService;

    public SendOutlineKeyCommand(IReplyService replyService, IUserService userService, IOutlineService outlineService, 
        ISubscriptionDateTimeService subscriptionDateTimeService, IOutlineKeysService outlineKeysService,
        ISubscriptionExtendService subscriptionExtendService)
    {
        _replyService = replyService;
        _userService = userService;
        _outlineService = outlineService;
        _subscriptionDateTimeService = subscriptionDateTimeService;
        _outlineKeysService = outlineKeysService;
        _subscriptionExtendService = subscriptionExtendService;
    }

    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        var telegramUser = message.From;
        var user = await _userService.GetAsync(telegramUser!.Id);
        
        if (user.OutlineKey is null)
        {
            await CreateOutlineKeyForUser(user);
            await _subscriptionExtendService.TryExtendForUser(user);
        }
        
        if (_subscriptionDateTimeService.IsSubscriptionActive(user.SubscriptionEndDate))
        {
            await _replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{user.OutlineKey!.AccessUrl}");
        }
        else
        {
            await _replyService.SendTextWithMainKeyboardAsync(message.Chat.Id,
                $"Ваш ключ временно деактивирован. Пополните счёт. Остаток: {user.MoneyInRoubles} рублей");
        }
    }

    private async Task CreateOutlineKeyForUser(User user)
    {
        var keyAccessUrl = _outlineService.GetNewKey(user.TelegramUserName, user.TelegramUserId);

        user.OutlineKey = await _outlineKeysService.CreateAsync(
            new OutlineKey
            {
                User = user, AccessUrl = keyAccessUrl
            });
    }
}