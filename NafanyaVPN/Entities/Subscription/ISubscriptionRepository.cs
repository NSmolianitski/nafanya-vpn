using NafanyaVPN.Entities.Subscription;

namespace NafanyaVPN.Database.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription> CreateAsync(Subscription model);
    IQueryable<Subscription> GetAll();
    Task<bool> DeleteAsync(Subscription model);
    Task<Subscription> UpdateAsync(Subscription model);
    Task UpdateAllAsync(IEnumerable<Subscription> models);
}