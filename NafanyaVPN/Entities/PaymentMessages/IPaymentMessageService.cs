using Telegram.Bot.Types;
using User = NafanyaVPN.Entities.Users.User;

namespace NafanyaVPN.Entities.Payments;

public interface IPaymentMessageService
{
    Task CreateAsync(Message message, User user);
    Task ClearPaymentMessageAsync(long userId);
}