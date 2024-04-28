using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionExtendService
{
    Task ExtendForAllUsers();
    Task TryExtendForUser(User user);
}