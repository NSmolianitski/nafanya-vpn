namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionRepository
{
    Task<Subscription> CreateAsync(Subscription model);
    IQueryable<Subscription> GetAll();
    Task<bool> DeleteAsync(Subscription model);
    Task<Subscription> UpdateAsync(Subscription model);
    Task UpdateAllAsync(IEnumerable<Subscription> models);
}