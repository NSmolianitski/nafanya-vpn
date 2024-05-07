using Telegram.Bot.Types;
using User = NafanyaVPN.Entities.Users.User;

namespace NafanyaVPN.Entities.PaymentMessages;

public interface IPaymentMessageService
{
    Task CreateAsync(Message message, User user);
    Task RemoveTelegramPaymentMessageAsync(long userId);
}