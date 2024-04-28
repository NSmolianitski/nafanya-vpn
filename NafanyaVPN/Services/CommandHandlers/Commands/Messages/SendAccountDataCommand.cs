using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService, IUserService userService)
    : ICommand<Telegram.Bot.Types.Message>
{
    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        var telegramUser = message.From;
        var user = await userService.GetAsync(telegramUser!.Id);

        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"Остаток средств: {user.MoneyInRoubles}");
    }
}