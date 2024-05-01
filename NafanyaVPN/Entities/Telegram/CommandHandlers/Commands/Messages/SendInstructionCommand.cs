using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendInstructionCommand(IReplyService replyService) : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        var instruction = "Текст инструкции";
        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{instruction}");
    }
}