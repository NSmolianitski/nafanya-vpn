using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class InstructionCommand(IReplyService replyService, IOutlineService outlineService) : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var instruction = outlineService.GetInstruction();
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, data.User.Subscription, 
            $"{instruction}");
    }
}