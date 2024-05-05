using NafanyaVPN.Entities.Outline;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendInstructionCommand(IReplyService replyService, IOutlineService outlineService) : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        var instruction = outlineService.GetInstruction();
        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{instruction}");
    }
}