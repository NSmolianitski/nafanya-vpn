namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionService
{
    Task<Subscription> GetAsync(string name);
    Task UpdateAsync(Subscription model);
}