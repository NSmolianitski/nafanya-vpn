using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public interface ISubscriptionExtendService
{
    Task TryExtendForAllUsers();
    Task TryExtendForUser(User user);
}