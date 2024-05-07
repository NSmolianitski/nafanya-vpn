using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendInstructionCommand(IReplyService replyService, IOutlineService outlineService) : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var instruction = outlineService.GetInstruction();
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{instruction}");
    }
}