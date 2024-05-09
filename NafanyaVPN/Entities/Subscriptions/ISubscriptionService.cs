namespace NafanyaVPN.Entities.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription> CreateAsync(Subscription model);
    Task<List<Subscription>> GetAllAsync();
    Task<List<Subscription>> GetAllNonExpiredAsync();
    Task<Subscription> UpdateAsync(Subscription model);
    Task UpdateAllAsync(IEnumerable<Subscription> models);
}