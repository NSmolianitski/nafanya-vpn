using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionExtendService
{
    Task TryExtendForAllUsers();
    Task TryExtendForUser(User user);
}