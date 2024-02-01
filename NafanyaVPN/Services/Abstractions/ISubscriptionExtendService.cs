using NafanyaVPN.Models;

namespace NafanyaVPN.Services.Abstractions;

public interface ISubscriptionExtendService
{
    Task ExtendForAllUsers();
    Task TryExtendForUser(User user);
}