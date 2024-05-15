using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionRenewService
{
    Task RenewAllNonExpiredAsync();
    Task RenewIfEnoughMoneyAsync(User user);
}