using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Users;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Остаток средств: {data.User.MoneyInRoubles}");
    }
}