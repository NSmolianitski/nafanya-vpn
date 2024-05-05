using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Users;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class DonateMoneyCommand(
    IReplyService replyService,
    IUserService userService,
    IPaymentService paymentService,
    ILogger<DonateMoneyCommand> logger)
    : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        var telegramUser = message.From;
        
        var user = await userService.GetAsync(telegramUser!.Id);
        const int sum = 100;
        user.MoneyInRoubles += sum;

        await userService.UpdateAsync(user);

        logger.LogInformation($"{user.TelegramUserName}({user.TelegramUserId}) пополнил счёт на {sum}. " +
                               $"Текущий баланс: {user.MoneyInRoubles}");
        
        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id,
            $"Счёт пополнен. Остаток средств: {user.MoneyInRoubles} рублей.");
    }
}