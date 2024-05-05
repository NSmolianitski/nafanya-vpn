using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Users;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService, IUserService userService)
    : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        var telegramUser = message.From;
        var user = await userService.GetAsync(telegramUser!.Id);

        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"Остаток средств: {user.MoneyInRoubles}");
    }
}