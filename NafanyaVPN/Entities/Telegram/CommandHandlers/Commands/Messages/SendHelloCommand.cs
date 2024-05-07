using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendHelloCommand(IReplyService replyService) : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        await replyService.SendHelloAsync(data.Message.Chat.Id);
    }
}