using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionExtendService
{
    Task RenewAllNonExpiredAsync();
    Task TryRenewForUserAsync(User user);
}