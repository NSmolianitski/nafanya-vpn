namespace NafanyaVPN.Entities.Subscriptions;

public interface ISubscriptionRepository
{
    Task<Subscription> CreateAsync(Subscription model);
    Task<List<Subscription>> GetAllAsync();
    Task<List<Subscription>> GetAllNonExpiredAsync();
    Task<bool> DeleteAsync(Subscription model);
    Task<Subscription> UpdateAsync(Subscription model);
    Task UpdateAllAsync(IEnumerable<Subscription> models);
}