using NafanyaVPN.Models;

namespace NafanyaVPN.Services.Abstractions;

public interface ISubscriptionService
{
    Task<Subscription> GetAsync(string name);
    Task UpdateAsync(Subscription model);
}