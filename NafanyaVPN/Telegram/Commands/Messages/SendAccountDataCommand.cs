using System.Globalization;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        
        string subscriptionMessage;
        if (user.OutlineKey == null || !user.OutlineKey!.Enabled) // TODO: заменить OutlineKey на Subscription
            subscriptionMessage = "Подписка неактивна";
        else
            subscriptionMessage = user.SubscriptionEndDate.ToString(CultureInfo.InvariantCulture);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Остаток средств: {user.MoneyInRoubles}\n" +
            $"Следующее обновление подписки: {subscriptionMessage}");
    }
}